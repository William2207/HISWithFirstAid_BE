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
                    SpecialtyId = d.PrimarySpecialtyId,
                    YearsOfExperience = d.YearsOfExperience
                })
                .ToList();
        }

        public async Task<int> GetDoctorIdByUserId(int userId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ tương ứng với người dùng.");
            return doctor.Id;
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

                if (schedule == null)
                    continue;

                var timeSlots = new List<string>();
                TimeSpan current = schedule.StartTime;
                while (current.Add(TimeSpan.FromHours(1)) <= schedule.EndTime)
                {
                    var end = current.Add(TimeSpan.FromHours(1));
                    timeSlots.Add($"{current:hh\\:mm} - {end:hh\\:mm}");
                    current = end;
                }

                var clinic = schedule.Clinic ?? defaultClinic;

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
