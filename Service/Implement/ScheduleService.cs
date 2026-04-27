using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Google.OrTools.Sat;
using FirstAidAPI.DTO.Doctor;

namespace FirstAidAPI.Service.Implement
{
    public class ScheduleService : IScheduleService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IShiftTypeRepository _shiftTypeRepository;
        private readonly IDoctorScheduleRepository _scheduleRepository;
        private readonly IClinicRepository _clinicRepository;

        public ScheduleService(
            IDoctorRepository doctorRepository,
            IShiftTypeRepository shiftTypeRepository,
            IDoctorScheduleRepository scheduleRepository,
            IClinicRepository clinicRepository)
        {
            _doctorRepository = doctorRepository;
            _shiftTypeRepository = shiftTypeRepository;
            _scheduleRepository = scheduleRepository;
            _clinicRepository = clinicRepository;
        }

        public async Task GenerateMonthlyScheduleAsync(int month, int year)
        {
            // 1. Kiểm tra đã có lịch tháng này chưa
            var exists = await _scheduleRepository.ExistsForMonthAsync(month, year);
            if (exists)
                await _scheduleRepository.DeleteByMonthAsync(month, year);

            // 2. Lấy data
            var doctors = await _doctorRepository.GetAllAsync();
            var shiftTypes = await _shiftTypeRepository.GetAllAsync();
            var nightShift = shiftTypes.First(s => s.IsNightShift);
            var dayShifts = shiftTypes.Where(s => !s.IsNightShift).ToList();

            var startDate = new DateOnly(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // 3. Khởi tạo OR-Tools
            var model = new CpModel();

            // 4. Tạo biến shifts[doctorId, day, shiftTypeId] = true/false
            var shifts = new Dictionary<(int doctorId, int day, int shiftTypeId), BoolVar>();
            foreach (var doctor in doctors)
                for (int day = 0; day < daysInMonth; day++)
                    foreach (var shift in shiftTypes)
                        shifts[(doctor.Id, day, shift.Id)] = model.NewBoolVar(
                            $"shift_d{doctor.Id}_day{day}_s{shift.Id}"
                        );

            // 5. Thêm constraints
            AddConstraints(model, shifts, doctors, shiftTypes, nightShift, dayShifts, daysInMonth, startDate);

            // 6. Chạy solver
            var solver = new CpSolver();
            var status = solver.Solve(model);

            if (status != CpSolverStatus.Feasible && status != CpSolverStatus.Optimal)
                throw new Exception("Không thể xếp lịch với các ràng buộc hiện tại!");

            await SaveScheduleAsync(solver, shifts, doctors, shiftTypes, nightShift, startDate, daysInMonth);
        }

        private void AddConstraints(
            CpModel model,
            Dictionary<(int, int, int), BoolVar> shifts,
            List<Doctor> doctors,
            List<ShiftType> shiftTypes,
            ShiftType nightShift,
            List<ShiftType> dayShifts,
            int daysInMonth,
            DateOnly startDate)
        {
            // ✅ Rule 1: Mỗi ca mỗi ngày chỉ có 1 bác sĩ trực theo từng khoa
            var specialtyGroups = doctors.GroupBy(d => d.SpecialtyId);
            foreach (var group in specialtyGroups)
            {
                var specialtyDoctors = group.ToList();
                for (int day = 0; day < daysInMonth; day++)
                    foreach (var shift in shiftTypes)
                    {
                        var date = startDate.AddDays(day);

                        // Chủ nhật không có ca nào
                        if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            foreach (var doctor in specialtyDoctors)
                                model.Add(shifts[(doctor.Id, day, shift.Id)] == 0);
                            continue;
                        }

                        // Thứ 7 chỉ có ca sáng
                        if (date.DayOfWeek == DayOfWeek.Saturday && shift.IsNightShift)
                        {
                            foreach (var doctor in specialtyDoctors)
                                model.Add(shifts[(doctor.Id, day, shift.Id)] == 0);
                            continue;
                        }

                        // Ca chiều thứ 7 cũng không có
                        if (date.DayOfWeek == DayOfWeek.Saturday &&
                            shift.StartTime >= new TimeSpan(15, 0, 0))
                        {
                            foreach (var doctor in specialtyDoctors)
                                model.Add(shifts[(doctor.Id, day, shift.Id)] == 0);
                            continue;
                        }

                        var vars = specialtyDoctors
                            .Select(d => shifts[(d.Id, day, shift.Id)])
                            .ToList();
                        model.AddExactlyOne(vars);
                    }
            }

            // ✅ Rule 2: Mỗi bác sĩ tối đa 1 ca/ngày
            foreach (var doctor in doctors)
                for (int day = 0; day < daysInMonth; day++)
                {
                    var vars = shiftTypes
                        .Select(s => shifts[(doctor.Id, day, s.Id)])
                        .Cast<ILiteral>()
                        .ToList();
                    model.AddAtMostOne(vars);
                }

            // ✅ Rule 3: Trực đêm → hôm sau nghỉ bù
            foreach (var doctor in doctors)
                for (int day = 0; day < daysInMonth - 1; day++)
                {
                    var nextDayVars = shiftTypes
                        .Select(s => (IntVar)shifts[(doctor.Id, day + 1, s.Id)])
                        .ToList();

                    model.Add(LinearExpr.Sum(nextDayVars) == 0)
                         .OnlyEnforceIf(shifts[(doctor.Id, day, nightShift.Id)]);
                }

            // ✅ Rule 4: Xoay tua ca đêm đều giữa các bác sĩ trong cùng khoa
            foreach (var group in specialtyGroups)
            {
                var specialtyDoctors = group.ToList();
                var nightCounts = specialtyDoctors.Select(d =>
                {
                    var vars = Enumerable.Range(0, daysInMonth)
                        .Select(day => (IntVar)shifts[(d.Id, day, nightShift.Id)])
                        .ToList();
                    return LinearExpr.Sum(vars);
                }).ToList();

                var maxNight = model.NewIntVar(0, daysInMonth, $"max_night_{group.Key}");
                var minNight = model.NewIntVar(0, daysInMonth, $"min_night_{group.Key}");
                model.AddMaxEquality(maxNight, nightCounts);
                model.AddMinEquality(minNight, nightCounts);
                model.Minimize(LinearExpr.WeightedSum(
                    new[] { maxNight, minNight },
                    new[] { 1L, -1L }
                ));
            }
        }

        private async Task SaveScheduleAsync(
            CpSolver solver,
            Dictionary<(int, int, int), BoolVar> shifts,
            List<Doctor> doctors,
            List<ShiftType> shiftTypes,
            ShiftType nightShift,
            DateOnly startDate,
            int daysInMonth)
        {
            var schedules = new List<DoctorSchedule>();
            var allClinics = await _clinicRepository.GetAllAsync();
            foreach (var doctor in doctors)
            {
                bool isOffNextDay = false;

                for (int day = 0; day < daysInMonth; day++)
                {
                    var date = startDate.AddDays(day);

                    // Ngày nghỉ bù sau ca đêm
                    if (isOffNextDay)
                    {
                        schedules.Add(new DoctorSchedule
                        {
                            DoctorId = doctor.Id,
                            Date = date,
                            IsOff = true
                        });
                        isOffNextDay = false;
                        continue;
                    }

                    foreach (var shift in shiftTypes)
                    {
                        if (solver.Value(shifts[(doctor.Id, day, shift.Id)]) == 1)
                        {
                            int? clinicId = null;
                            int? specialtyId = null;

                            if (shift.IsNightShift)
                                specialtyId = doctor.SpecialtyId;
                            else
                                clinicId = allClinics.FirstOrDefault(c => c.SpecialtyId == doctor.SpecialtyId)?.Id;

                            schedules.Add(new DoctorSchedule
                            {
                                DoctorId = doctor.Id,
                                ShiftTypeId = shift.Id,
                                Date = date,
                                ClinicId = clinicId,
                                SpecialtyId = specialtyId,
                                IsOff = false
                            });

                            if (shift.IsNightShift)
                                isOffNextDay = true;
                        }
                    }
                }
            }

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

        private DoctorScheduleDto MapToDto(DoctorSchedule s) => new DoctorScheduleDto
        {
            DoctorId = s.DoctorId,
            DoctorName = s.Doctor.User.FullName,
            Date = s.Date,
            ShiftName = s.ShiftType?.Name,
            ClinicRoom = s.Clinic?.RoomNumber,
            SpecialtyId = s.SpecialtyId,
            IsOff = s.IsOff
        };
    }
}
