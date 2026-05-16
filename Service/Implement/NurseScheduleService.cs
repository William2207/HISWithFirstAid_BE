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

            // Round Robin: chia 2 nhóm xoay vòng ca đêm
            var shuffled = nurses.OrderBy(_ => Guid.NewGuid()).ToList();
            var groupA = shuffled.Take(nurses.Count / 2).ToList();
            var groupB = shuffled.Skip(nurses.Count / 2).ToList();

            for (int dayIdx = 0; dayIdx < daysInMonth; dayIdx++)
            {
                var date = startDate.AddDays(dayIdx);

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                bool hasNightShift = date.DayOfWeek != DayOfWeek.Saturday;

                var nightGroup = (dayIdx % 2 == 0) ? groupA : groupB;
                var dayGroup = (dayIdx % 2 == 0) ? groupB : groupA;

                var assignedToday = new HashSet<int>();

                // ── PASS 1: Ca đêm ──────────────────────────────────────────
                if (hasNightShift)
                {
                    // Lọc người còn dưới giới hạn 10 ca đêm/tháng
                    // Nếu nightGroup hết quota thì lấy từ dayGroup (fallback)
                    var nightCandidates = nightGroup
                        .Concat(dayGroup)
                        .Where(n => nightShiftCount[n.Id] < MaxNightShiftsPerMonth)
                        .OrderBy(n => nightShiftCount[n.Id])  // Ưu tiên người ít ca đêm nhất
                        .ThenBy(_ => Guid.NewGuid())
                        .ToList();

                    // Pass 1a: Tối thiểu 1 người/ward (nếu còn người)
                    foreach (var ward in wards)
                    {
                        var nurse = nightCandidates
                            .FirstOrDefault(n => !assignedToday.Contains(n.Id));

                        if (nurse == null) break; // Không đủ người → bỏ qua, không throw

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
                        assignedToday.Add(nurse.Id);
                    }

                    // Pass 1b: Thêm người thứ 2/ward nếu còn người chưa đủ quota
                    foreach (var ward in wards)
                    {
                        var secondNurse = nightCandidates
                            .FirstOrDefault(n => !assignedToday.Contains(n.Id));

                        if (secondNurse == null) break;

                        schedules.Add(new NurseSchedule
                        {
                            NurseId = secondNurse.Id,
                            Date = date,
                            IsNightShift = true,
                            WardId = ward.Id,
                            SpecialtyId = null,
                            IsOff = false
                        });

                        nightShiftCount[secondNurse.Id]++;
                        assignedToday.Add(secondNurse.Id);
                    }
                }

                // ── PASS 2: Ca ngày ─────────────────────────────────────────
                foreach (var ward in wards)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        // Ưu tiên dayGroup (không làm đêm hôm nay), sau đó mới lấy nightGroup
                        var nurse = dayGroup
                            .Concat(nightGroup)
                            .Where(n => !assignedToday.Contains(n.Id))
                            .OrderBy(n => dayShiftCount[n.Id])
                            .ThenBy(_ => Guid.NewGuid())
                            .FirstOrDefault();

                        if (nurse == null) break;

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
                        assignedToday.Add(nurse.Id);
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
