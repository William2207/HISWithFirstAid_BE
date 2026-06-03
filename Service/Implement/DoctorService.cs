using FirstAidAPI.DTO.Doctor;
using FirstAidAPI.Repository;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FirstAidAPI.Service.Implement
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<User> _userManager;

        public DoctorService(IDoctorRepository doctorRepository, UserManager<User> userManager)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
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
                    Specialty = d.Specialty?.Name ?? string.Empty,
                    SpecialtyId = d.SpecialtyId,
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

        public async Task<DoctorProfileDto?> GetDoctorProfileAsync(int userId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor == null) return null;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return new DoctorProfileDto
            {
                Id = doctor.Id,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                IdCard = user.IdCard,
                SpecialtyId = doctor.SpecialtyId,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                LicenseNumber = doctor.LicenseNumber,
                Qualifications = doctor.Qualifications,
                YearsOfExperience = doctor.YearsOfExperience,
                IsHeadDoctor = doctor.Specialty?.HeadDoctorId == doctor.Id
            };
        }

        public async Task<bool> UpdateDoctorProfileAsync(int userId, UpdateDoctorProfileDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            if (updateDto.FullName != null) user.FullName = updateDto.FullName;
            if (updateDto.PhoneNumber != null) user.PhoneNumber = updateDto.PhoneNumber;
            if (updateDto.DateOfBirth.HasValue) user.DateOfBirth = DateTime.SpecifyKind(updateDto.DateOfBirth.Value, DateTimeKind.Utc);
            if (updateDto.Gender != null) user.Gender = updateDto.Gender;
            if (updateDto.Address != null) user.Address = updateDto.Address;
            if (updateDto.IdCard != null) user.IdCard = updateDto.IdCard;

            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded) return false;

            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor != null)
            {
                if (updateDto.LicenseNumber != null) doctor.LicenseNumber = updateDto.LicenseNumber;
                if (updateDto.Qualifications != null) doctor.Qualifications = updateDto.Qualifications;
                if (updateDto.YearsOfExperience.HasValue) doctor.YearsOfExperience = updateDto.YearsOfExperience.Value;

                await _doctorRepository.UpdateAsync(doctor);
            }

            return true;
        }

        public async Task<List<DoctorAvailabilityDTO>> GetAvailableDoctorsAsync(int specialtyId, DateTime date)
        {
            var doctors = await _doctorRepository.GetDoctorsBySpecialtyForBookingAsync(specialtyId);
            var result = new List<DoctorAvailabilityDTO>();
            var targetDate = DateOnly.FromDateTime(date);

            // Lấy default clinic
            var defaultClinic = await _doctorRepository.GetDefaultClinicBySpecialtyAsync(specialtyId);

            foreach (var doc in doctors)
            {
                // Tìm lịch theo ngày của bác sĩ
                var schedules = doc.Schedules.Where(s => s.Date == targetDate && !s.IsOff).ToList();

                if (!schedules.Any())
                    continue;

                var timeSlots = new List<string>();
                foreach (var schedule in schedules)
                {
                    if (schedule.IsNightShift) continue; // Bỏ qua ca đêm khi tìm slot đặt khám thường
                    
                    // Ca sáng: 08:00 - 12:00
                    // Ca chiều: 13:00 - 17:00
                    // Tạm thời fix cứng slot cho ca ngày
                    timeSlots.AddRange(new[] { 
                        "08:00 - 09:00", "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                        "13:00 - 14:00", "14:00 - 15:00", "15:00 - 16:00", "16:00 - 17:00"
                    });
                }

                if (!timeSlots.Any())
                    continue;

                var firstSchedule = schedules.First();
                var clinic = firstSchedule.Clinic ?? defaultClinic;

                result.Add(new DoctorAvailabilityDTO
                {
                    Id = doc.Id,
                    Name = doc.User?.FullName ?? string.Empty,
                    SpecialtyName = doc.Specialty?.Name ?? string.Empty,
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
