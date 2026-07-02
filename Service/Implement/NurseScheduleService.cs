using FirstAidAPI.DTO.Nurse;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class NurseScheduleService : INurseScheduleService
    {
        private readonly INurseRepository _nurseRepository;
        private readonly INurseScheduleRepository _scheduleRepository;
        private readonly IWardRepository _wardRepository;
        private const int MaxNightShiftsPerMonth = 10;

        public NurseScheduleService(
            INurseRepository nurseRepository,
            INurseScheduleRepository scheduleRepository,
            IWardRepository wardRepository)
        {
            _nurseRepository = nurseRepository;
            _scheduleRepository = scheduleRepository;
            _wardRepository = wardRepository;
        }

        public async Task GenerateMonthlyScheduleAsync(int month, int year)
        {
            var exists = await _scheduleRepository.ExistsForMonthAsync(month, year);
            if (exists)
                await _scheduleRepository.DeleteByMonthAsync(month, year);

            var allNurses = await _nurseRepository.GetAllAsync();
            var specialtyIds = allNurses.Select(n => n.SpecialityId).Distinct().ToList();

            var all = new List<NurseSchedule>();
            foreach (var specialtyId in specialtyIds)
            {
                var nurses = allNurses.Where(n => n.SpecialityId == specialtyId).ToList();
                var wards = await _wardRepository.GetBySpecialtyAsync(specialtyId);
                var generated = GenerateForSpecialty(specialtyId, nurses, wards, month, year);
                all.AddRange(generated);
            }

            await _scheduleRepository.AddRangeAsync(all);
        }

        public async Task GenerateSpecialtyScheduleAsync(int specialtyId, int month, int year)
        {
            var exists = await _scheduleRepository.ExistsForSpecialtyAndMonthAsync(specialtyId, month, year);
            if (exists)
                await _scheduleRepository.DeleteBySpecialtyAndMonthAsync(specialtyId, month, year);

            var allNurses = await _nurseRepository.GetAllAsync();
            var nurses = allNurses.Where(n => n.SpecialityId == specialtyId).ToList();

            if (!nurses.Any())
                throw new Exception("Không có y tá nào trong khoa để xếp lịch!");

            var wards = await _wardRepository.GetBySpecialtyAsync(specialtyId);

            if (!wards.Any())
                throw new Exception("Không có phòng bệnh nào trong khoa để xếp lịch!");

            var schedules = GenerateForSpecialty(specialtyId, nurses, wards, month, year);
            await _scheduleRepository.AddRangeAsync(schedules);
        }

        public async Task<List<NurseScheduleDto>> GetMonthlyScheduleAsync(int month, int year)
        {
            var schedules = await _scheduleRepository.GetByMonthAsync(month, year);
            return schedules.Select(MapToDto).ToList();
        }

        public async Task<List<NurseScheduleDto>> GetNurseScheduleAsync(int nurseId, int month, int year)
        {
            var schedules = await _scheduleRepository.GetByNurseAndMonthAsync(nurseId, month, year);
            return schedules.Select(MapToDto).ToList();
        }

        public async Task<List<NurseScheduleDto>> GetSpecialtyScheduleAsync(int specialtyId, int month, int year)
        {
            var schedules = await _scheduleRepository.GetBySpecialtyAndMonthAsync(specialtyId, month, year);
            return schedules.Select(MapToDto).ToList();
        }

        private List<NurseSchedule> GenerateForSpecialty(
            int specialtyId,
            List<Nurse> nurses,
            List<Ward> wards,
            int month,
            int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var startDate = new DateOnly(year, month, 1);

            var dayShiftCount = nurses.ToDictionary(n => n.Id, _ => 0);
            var nightShiftCount = nurses.ToDictionary(n => n.Id, _ => 0);
            var schedules = new List<NurseSchedule>();

            for (int dayIdx = 0; dayIdx < daysInMonth; dayIdx++)
            {
                var date = startDate.AddDays(dayIdx);

                bool hasNightShift = date.DayOfWeek != DayOfWeek.Saturday;

                // ── PASS 1: Ca ngày (2 nurses/ward, có thể trực nhiều ward) ──
                // Mỗi ward cần 2 nurse → tổng slot ca ngày = wards.Count * 2
                // Nurse ít ca ngày nhất được ưu tiên, và 1 nurse có thể trực nhiều ward
                var daySlots = wards.Count * 2;

                // Round Robin: lấy tuần tự theo số ca ngày tăng dần, lặp lại nếu hết người
                var dayRanked = nurses
                    .OrderBy(n => dayShiftCount[n.Id])
                    .ThenBy(_ => Guid.NewGuid())
                    .ToList();

                for (int slot = 0; slot < daySlots; slot++)
                {
                    var ward = wards[slot / 2];           // Mỗi ward 2 slot: slot 0,1 → ward[0], slot 2,3 → ward[1]...
                    var nurse = dayRanked[slot % dayRanked.Count]; // Xoay vòng nếu hết người

                    schedules.Add(new NurseSchedule
                    {
                        NurseId = nurse.Id,
                        Date = date,
                        IsNightShift = false,
                        WardId = ward.Id,
                        SpecialtyId = null,
                        IsOff = false
                    });

                    dayShiftCount[nurse.Id]++;
                }

                // ── PASS 2: Ca đêm (tối thiểu 1, tối đa 2 nurses/ward) ──
                if (hasNightShift)
                {
                    // Chỉ lấy người còn dưới quota 10 ca đêm/tháng
                    // Ưu tiên người ít ca đêm nhất, xoay vòng nếu hết người
                    var nightRanked = nurses
                        .Where(n => nightShiftCount[n.Id] < MaxNightShiftsPerMonth)
                        .OrderBy(n => nightShiftCount[n.Id])
                        .ThenBy(_ => Guid.NewGuid())
                        .ToList();

                    if (nightRanked.Count == 0)
                    {
                        // Tất cả đã hết quota → reset người ít ca nhất làm tiếp
                        // (trường hợp hiếm, tháng quá dài hoặc nurses quá ít)
                        nightRanked = nurses
                            .OrderBy(n => nightShiftCount[n.Id])
                            .ThenBy(_ => Guid.NewGuid())
                            .ToList();
                    }

                    // Pass 2a: Bắt buộc 1 nurse/ward
                    for (int i = 0; i < wards.Count; i++)
                    {
                        var nurse = nightRanked[i % nightRanked.Count]; // Xoay vòng
                        var ward = wards[i];

                        schedules.Add(new NurseSchedule
                        {
                            NurseId = nurse.Id,
                            Date = date,
                            IsNightShift = true,
                            WardId = ward.Id,
                            SpecialtyId = null,
                            IsOff = false
                        });

                        nightShiftCount[nurse.Id]++;
                    }

                    // Pass 2b: Thêm nurse thứ 2/ward nếu còn người chưa hết quota
                    var secondNightRanked = nurses
                        .Where(n => nightShiftCount[n.Id] < MaxNightShiftsPerMonth)
                        .OrderBy(n => nightShiftCount[n.Id])
                        .ThenBy(_ => Guid.NewGuid())
                        .ToList();

                    for (int i = 0; i < wards.Count; i++)
                    {
                        if (secondNightRanked.Count == 0) break;

                        var nurse = secondNightRanked[i % secondNightRanked.Count];
                        var ward = wards[i];

                        schedules.Add(new NurseSchedule
                        {
                            NurseId = nurse.Id,
                            Date = date,
                            IsNightShift = true,
                            WardId = ward.Id,
                            SpecialtyId = null,
                            IsOff = false
                        });

                        nightShiftCount[nurse.Id]++;
                    }
                }
            }

            return schedules;
        }

        private static NurseScheduleDto MapToDto(NurseSchedule s) => new NurseScheduleDto
        {
            NurseId = s.NurseId,
            NurseName = s.Nurse?.User?.FullName ?? string.Empty,
            Date = s.Date,
            ShiftName = s.IsOff
                ? "Nghỉ bù"
                : s.IsNightShift
                    ? "Ca đêm"
                    : "Ca ngày",
            WardRoom = s.Ward?.RoomNumber,
            SpecialtyId = s.SpecialtyId,
            IsOff = s.IsOff
        };
    }
}
