using FirstAidAPI.DTO.Doctor;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorLookupDTO>> GetDoctorsForLookupAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return doctors
                .Where(d => d.IsAvailable)
                .Select(d => new DoctorLookupDTO
                {
                    Id = d.Id,
                    Name = d.User?.FullName ?? string.Empty,
                    Specialty = d.PrimarySpecialty?.Name ?? string.Empty,
                    YearsOfExperience = d.YearsOfExperience
                })
                .ToList();
        }

        public async Task<List<DoctorAvailabilityDTO>> GetAvailableDoctorsAsync(int specialtyId, DateTime date)
        {
            var doctors = await _doctorRepository.GetDoctorsBySpecialtyForBookingAsync(specialtyId);
            var result = new List<DoctorAvailabilityDTO>();
            var dayOfWeek = date.DayOfWeek;

            // Lấy default clinic
            var defaultClinic = await _doctorRepository.GetDefaultClinicBySpecialtyAsync(specialtyId);

            foreach (var doc in doctors)
            {
                // Tìm lịch theo ngày của bác sĩ
                var schedule = doc.Schedules.FirstOrDefault(s => s.DayOfWeek == dayOfWeek && s.IsAvailable);

                // Setup mốc thời gian. Nếu bác sĩ có lịch, chia slot mỗi 1 tiếng theo khoản T
                var timeSlots = new List<string>();
                if (schedule != null)
                {
                    TimeSpan current = schedule.StartTime;
                    while (current.Add(TimeSpan.FromHours(1)) <= schedule.EndTime)
                    {
                        var end = current.Add(TimeSpan.FromHours(1));
                        timeSlots.Add($"{current:hh\\:mm} - {end:hh\\:mm}");
                        current = end;
                    }
                }
                else
                {
                    
                    timeSlots = new List<string> {
                        "07:30 - 08:30",
                        "08:30 - 09:30",
                        "09:30 - 10:30",
                        "10:30 - 11:30"
                    };
                }

                var clinic = schedule?.Clinic ?? defaultClinic;

                result.Add(new DoctorAvailabilityDTO
                {
                    Id = doc.Id,
                    Name = doc.User?.FullName ?? string.Empty,
                    SpecialtyName = doc.PrimarySpecialty?.Name ?? string.Empty,
                    ClinicId = clinic?.Id,
                    ClinicRoom = clinic?.RoomNumber ?? "Chưa sắp phòng",
                    ClinicFloor = clinic?.Floor,
                    AvailableTimeSlots = timeSlots
                });
            }

            return result;
        }
    }
}
