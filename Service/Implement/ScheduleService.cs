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
        private readonly IWardRepository _wardRepository;

        public ScheduleService(
            IDoctorRepository doctorRepository,
            IDoctorScheduleRepository scheduleRepository,
            IClinicRepository clinicRepository,
            IWardRepository wardRepository)
        {
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
            _clinicRepository = clinicRepository;
            _wardRepository = wardRepository;
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
                var wards = await _wardRepository.GetBySpecialtyAsync(specialtyId);
                var generated = GenerateForSpecialty(specialtyId, doctors, clinics, wards, month, year);
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
            var wards = await _wardRepository.GetBySpecialtyAsync(specialtyId);

            if (!clinics.Any() && !wards.Any())
                throw new Exception("Không có phòng khám hay phòng bệnh nào trong khoa để xếp lịch!");

            var schedules = GenerateForSpecialty(specialtyId, doctors, clinics, wards, month, year);
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
    List<Ward> wards,
    int month,
    int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var startDate = new DateOnly(year, month, 1);

            var dayShiftCount = doctors.ToDictionary(d => d.Id, _ => 0);
            var nightShiftCount = doctors.ToDictionary(d => d.Id, _ => 0);
            var restingOnDate = new Dictionary<DateOnly, HashSet<int>>();
            var schedules = new List<DoctorSchedule>();

            for (int dayIdx = 0; dayIdx < daysInMonth; dayIdx++)
            {
                var date = startDate.AddDays(dayIdx);

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                bool hasNightShift = date.DayOfWeek != DayOfWeek.Saturday;

                var resting = restingOnDate.GetValueOrDefault(date, new HashSet<int>());

                // Ghi nhận nghỉ bù
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

                var available = doctors
                    .Where(d => !resting.Contains(d.Id))
                    .ToList();

                // Số bác sĩ tối thiểu cần: clinics + 1 ca đêm
                int minNeeded = clinics.Count + (hasNightShift ? 1 : 0);
                if (available.Count < minNeeded)
                {
                    // Không đủ bác sĩ tối thiểu → log/throw tuỳ bạn
                    continue;
                }

                var assignedToday = new HashSet<int>();

                // ── Ưu tiên 1: Ca đêm (reserve trước để không bị lấy mất) ──
                Doctor nightDoctor = null;
                if (hasNightShift)
                {
                    nightDoctor = available
                        .OrderBy(d => nightShiftCount[d.Id])
                        .ThenBy(d => d.Id)
                        .First();

                    // Reserve trước, chưa add schedule
                    assignedToday.Add(nightDoctor.Id);
                }

                // ── Ưu tiên 2: Mỗi clinic 1 bác sĩ (bắt buộc) ──
                foreach (var clinic in clinics)
                {
                    var doctor = available
                        .Where(d => !assignedToday.Contains(d.Id))
                        .OrderBy(d => dayShiftCount[d.Id])
                        .ThenBy(d => d.Id)
                        .FirstOrDefault();

                    if (doctor == null) break;

                    schedules.Add(new DoctorSchedule
                    {
                        DoctorId = doctor.Id,
                        Date = date,
                        IsNightShift = false,
                        ClinicId = clinic.Id,
                        WardId = null,
                        SpecialtyId = null,
                        IsOff = false
                    });

                    dayShiftCount[doctor.Id]++;
                    assignedToday.Add(doctor.Id);
                }

                // ── Ưu tiên 3: Ward — bác sĩ còn lại chia nhau trực ──
                // Lấy tất cả bác sĩ còn rảnh (không tính nightDoctor)
                var allWardCandidates = available
    .Where(d => !assignedToday.Contains(d.Id))
    .OrderBy(d => dayShiftCount[d.Id])
    .ThenBy(d => d.Id)
    .ToList();

                // Không cần nhiều hơn số ward
                int wardDoctorCount = Math.Min(allWardCandidates.Count, wards.Count);
                var wardDoctors = allWardCandidates.Take(wardDoctorCount).ToList();

                if (wardDoctors.Count > 0)
                {
                    for (int i = 0; i < wards.Count; i++)
                    {
                        var doctor = wardDoctors[i % wardDoctors.Count];

                        schedules.Add(new DoctorSchedule
                        {
                            DoctorId = doctor.Id,
                            Date = date,
                            IsNightShift = false,
                            ClinicId = null,
                            WardId = wards[i].Id,
                            SpecialtyId = null,
                            IsOff = false
                        });
                    }

                    foreach (var d in wardDoctors)
                        dayShiftCount[d.Id]++;
                }

                // ── Add ca đêm (đã reserve từ trước) ──
                if (nightDoctor != null)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        DoctorId = nightDoctor.Id,
                        Date = date,
                        IsNightShift = true,
                        ClinicId = null,
                        WardId = null,
                        SpecialtyId = specialtyId,
                        IsOff = false
                    });

                    nightShiftCount[nightDoctor.Id]++;

                    // Nghỉ bù ngày hôm sau
                    var nextDate = date.AddDays(1);
                    if (!restingOnDate.ContainsKey(nextDate))
                        restingOnDate[nextDate] = new HashSet<int>();
                    restingOnDate[nextDate].Add(nightDoctor.Id);
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
            WardRoom = s.Ward?.RoomNumber,
            SpecialtyId = s.SpecialtyId,
            IsOff = s.IsOff
        };
    }
}
