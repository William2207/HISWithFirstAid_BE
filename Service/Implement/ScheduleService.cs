using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using FirstAidAPI.DTO.Doctor;

namespace FirstAidAPI.Service.Implement
{
    /// <summary>
    /// Xếp lịch làm việc theo thuật toán Round-Robin công bằng.
    ///
    /// Quy tắc:
    ///   • Thứ 2 – Thứ 7 (ngày làm việc):
    ///       - Mỗi phòng khám (Clinic) trong khoa được giao đúng 1 bác sĩ (ca ngày).
    ///       - Thứ 2 – Thứ 6: thêm 1 bác sĩ trực đêm cho cả khoa.
    ///       - Thứ 7: chỉ có ca ngày, không có ca đêm.
    ///   • Chủ nhật: không xếp ca.
    ///   • Sau ca đêm, bác sĩ được nghỉ bù ngày hôm sau.
    ///   • Phân công xoay vòng theo số ca đã nhận – bác sĩ có ít ca nhất được ưu tiên.
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorScheduleRepository _scheduleRepository;
        private readonly IClinicRepository _clinicRepository;

        public ScheduleService(
            IDoctorRepository doctorRepository,
            IDoctorScheduleRepository scheduleRepository,
            IClinicRepository clinicRepository)
        {
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
            _clinicRepository = clinicRepository;
        }

        // ─────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────

        public async Task GenerateMonthlyScheduleAsync(int month, int year)
        {
            // Xóa lịch cũ toàn hệ thống
            var exists = await _scheduleRepository.ExistsForMonthAsync(month, year);
            if (exists)
                await _scheduleRepository.DeleteByMonthAsync(month, year);

            var allDoctors = await _doctorRepository.GetAllAsync();
            var specialtyIds = allDoctors.Select(d => d.SpecialtyId).Distinct().ToList();

            var all = new List<DoctorSchedule>();
            foreach (var specialtyId in specialtyIds)
            {
                var doctors = allDoctors.Where(d => d.SpecialtyId == specialtyId).ToList();
                var clinics = await _clinicRepository.GetBySpecialtyAsync(specialtyId);
                var generated = GenerateForSpecialty(specialtyId, doctors, clinics, month, year);
                all.AddRange(generated);
            }

            await _scheduleRepository.AddRangeAsync(all);
        }

        public async Task GenerateSpecialtyScheduleAsync(int specialtyId, int month, int year)
        {
            // Xóa lịch cũ của khoa này
            var exists = await _scheduleRepository.ExistsForSpecialtyAndMonthAsync(specialtyId, month, year);
            if (exists)
                await _scheduleRepository.DeleteBySpecialtyAndMonthAsync(specialtyId, month, year);

            var allDoctors = await _doctorRepository.GetAllAsync();
            var doctors = allDoctors.Where(d => d.SpecialtyId == specialtyId).ToList();

            if (!doctors.Any())
                throw new Exception("Không có bác sĩ nào trong khoa để xếp lịch!");

            var clinics = await _clinicRepository.GetBySpecialtyAsync(specialtyId);
            if (!clinics.Any())
                throw new Exception("Không có phòng khám nào trong khoa để xếp lịch!");

            var schedules = GenerateForSpecialty(specialtyId, doctors, clinics, month, year);
            await _scheduleRepository.AddRangeAsync(schedules);
        }

        public async Task<List<DoctorScheduleDto>> GetMonthlyScheduleAsync(int month, int year)
        {
            var schedules = await _scheduleRepository.GetByMonthAsync(month, year);
            return schedules.Select(MapToDto).ToList();
        }

        public async Task<List<DoctorScheduleDto>> GetDoctorScheduleAsync(int doctorId, int month, int year)
        {
            var schedules = await _scheduleRepository.GetByDoctorAndMonthAsync(doctorId, month, year);
            return schedules.Select(MapToDto).ToList();
        }

        public async Task<List<DoctorScheduleDto>> GetSpecialtyScheduleAsync(int specialtyId, int month, int year)
        {
            var schedules = await _scheduleRepository.GetBySpecialtyAndMonthAsync(specialtyId, month, year);
            return schedules.Select(MapToDto).ToList();
        }

        // ─────────────────────────────────────────────────────────────────
        //  Core generation algorithm (Round-Robin)
        // ─────────────────────────────────────────────────────────────────

        private List<DoctorSchedule> GenerateForSpecialty(
            int specialtyId,
            List<Doctor> doctors,
            List<Clinic> clinics,
            int month,
            int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var startDate = new DateOnly(year, month, 1);

            // Đếm số ca để phân công công bằng
            var dayShiftCount = doctors.ToDictionary(d => d.Id, _ => 0);
            var nightShiftCount = doctors.ToDictionary(d => d.Id, _ => 0);

            // restingOnDate[date] = tập bác sĩ phải nghỉ bù ngày đó (sau ca đêm)
            var restingOnDate = new Dictionary<DateOnly, HashSet<int>>();

            var schedules = new List<DoctorSchedule>();

            for (int dayIdx = 0; dayIdx < daysInMonth; dayIdx++)
            {
                var date = startDate.AddDays(dayIdx);

                // Chủ nhật không làm việc
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                // Thứ 7 không có ca đêm
                bool hasNightShift = date.DayOfWeek != DayOfWeek.Saturday;

                // Bác sĩ đang nghỉ bù hôm nay
                var resting = restingOnDate.GetValueOrDefault(date, new HashSet<int>());

                // Ghi nhận bản ghi nghỉ bù
                foreach (var restingDoctorId in resting)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        DoctorId = restingDoctorId,
                        Date = date,
                        IsOff = true,
                        IsNightShift = false,
                        SpecialtyId = specialtyId
                    });
                }

                // Bác sĩ có thể làm việc hôm nay
                var available = doctors
                    .Where(d => !resting.Contains(d.Id))
                    .ToList();

                // Kiểm tra đủ bác sĩ cho các phòng khám
                int needed = clinics.Count + (hasNightShift ? 1 : 0);
                if (available.Count < clinics.Count)
                {
                    // Không đủ bác sĩ: gán luân phiên hết những gì có, bỏ qua ca đêm
                    // (tránh crash — thực tế nên có đủ bác sĩ)
                    needed = Math.Min(available.Count, clinics.Count);
                }

                // ── Ca ngày: mỗi phòng khám 1 bác sĩ ──
                var assignedToday = new HashSet<int>();

                foreach (var clinic in clinics)
                {
                    var doctor = available
                        .Where(d => !assignedToday.Contains(d.Id))
                        .OrderBy(d => dayShiftCount[d.Id])   // ưu tiên người ít ca nhất
                        .ThenBy(d => d.Id)                   // tiebreak ổn định
                        .FirstOrDefault();

                    if (doctor == null) break; // không còn bác sĩ rảnh

                    schedules.Add(new DoctorSchedule
                    {
                        DoctorId = doctor.Id,
                        Date = date,
                        IsNightShift = false,
                        ClinicId = clinic.Id,
                        SpecialtyId = null,
                        IsOff = false
                    });

                    dayShiftCount[doctor.Id]++;
                    assignedToday.Add(doctor.Id);
                }

                // ── Ca đêm: 1 bác sĩ cho toàn khoa ──
                if (hasNightShift)
                {
                    var nightDoctor = available
                        .Where(d => !assignedToday.Contains(d.Id))
                        .OrderBy(d => nightShiftCount[d.Id])  // ưu tiên người ít trực đêm nhất
                        .ThenBy(d => d.Id)
                        .FirstOrDefault();

                    if (nightDoctor != null)
                    {
                        schedules.Add(new DoctorSchedule
                        {
                            DoctorId = nightDoctor.Id,
                            Date = date,
                            IsNightShift = true,
                            ClinicId = null,
                            SpecialtyId = specialtyId,
                            IsOff = false
                        });

                        nightShiftCount[nightDoctor.Id]++;

                        // Bác sĩ trực đêm nghỉ bù ngày hôm sau
                        var nextDate = date.AddDays(1);
                        if (!restingOnDate.ContainsKey(nextDate))
                            restingOnDate[nextDate] = new HashSet<int>();
                        restingOnDate[nextDate].Add(nightDoctor.Id);
                    }
                }
            }

            return schedules;
        }

        // ─────────────────────────────────────────────────────────────────
        //  DTO mapping
        // ─────────────────────────────────────────────────────────────────

        private static DoctorScheduleDto MapToDto(DoctorSchedule s) => new DoctorScheduleDto
        {
            DoctorId = s.DoctorId,
            DoctorName = s.Doctor?.User?.FullName ?? string.Empty,
            Date = s.Date,
            ShiftName = s.IsOff
                ? "Nghỉ bù"
                : s.IsNightShift
                    ? "Ca đêm"
                    : "Ca ngày",
            ClinicRoom = s.Clinic?.RoomNumber,
            SpecialtyId = s.SpecialtyId,
            IsOff = s.IsOff
        };
    }
}
