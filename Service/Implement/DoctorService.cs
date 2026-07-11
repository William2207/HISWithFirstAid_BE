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
        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorService(IDoctorRepository doctorRepository, UserManager<User> userManager, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;
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
            if (updateDto.Avatar != null) user.Avatar = updateDto.Avatar;
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

        public async Task<List<DoctorAvailabilityDTO>> GetAvailableDoctorsAsync(int specialtyId, DateTime date, bool isReceptionist = false)
        {
            var doctors = await _doctorRepository.GetDoctorsBySpecialtyForBookingAsync(specialtyId);
            // Lọc bỏ schedules không có clinic
            foreach (var doctor in doctors)
            {
                doctor.Schedules = doctor.Schedules
                    .Where(s => s.ClinicId != null)
                    .ToList();
            }

            // Bỏ luôn bác sĩ không còn schedule nào sau khi lọc
            doctors = doctors.Where(d => d.Schedules.Any()).ToList();
            var result = new List<DoctorAvailabilityDTO>();
            var targetDate = DateOnly.FromDateTime(date);

            foreach (var doc in doctors)
            {
                // Tìm lịch theo ngày của bác sĩ
                var schedules = doc.Schedules.Where(s => s.Date == targetDate && !s.IsOff).ToList();

                if (!schedules.Any())
                    continue;

                // Tối ưu hóa: Lấy tất cả lịch hẹn trong ngày của bác sĩ và gom nhóm theo giờ để truy vấn O(1)
                var appointmentsForDay = await _appointmentRepository.GetAppointmentsByDoctorAndDateAsync(doc.Id, date);
                var onlineAppointmentsCountByHour = appointmentsForDay
                    .Where(a => a.Type == FirstAidAPI.Enums.AppointmentType.Online)
                    .GroupBy(a => a.AppointmentDateTime.TimeOfDay)
                    .ToDictionary(g => g.Key, g => g.Count());

                var timeSlots = new List<TimeSlotDTO>();
                foreach (var schedule in schedules)
                {
                    if (schedule.IsNightShift) continue; // Bỏ qua ca đêm khi tìm slot đặt khám thường

                    // Ca sáng: 08:00 - 12:00
                    // Ca chiều: 13:00 - 17:00
                    var potentialSlots = new[] {
                        "08:00 - 09:00", "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
                        "13:00 - 14:00", "14:00 - 15:00", "15:00 - 16:00", "16:00 - 17:00"
                    };

                    foreach (var slotStr in potentialSlots)
                    {
                        var startTimeStr = slotStr.Split(" - ")[0]; // "08:00"
                        if (TimeSpan.TryParse(startTimeStr, out var startTimeSpan))
                        {
                            var onlineCount = onlineAppointmentsCountByHour.TryGetValue(startTimeSpan, out int count) ? count : 0;
                            var isAvailable = onlineCount < 10;
                            
                            // Nếu là receptionist, họ có thể đặt lịch walk-in nên coi như luôn available trên UI của họ (sẽ chặn ở BE nếu walkInCount >= 10).
                            // Còn bệnh nhân sẽ thấy isAvailable = false nếu onlineCount >= 10.
                            timeSlots.Add(new TimeSlotDTO
                            {
                                Time = slotStr,
                                IsAvailable = isReceptionist || isAvailable
                            });
                        }
                    }
                }

                if (!timeSlots.Any())
                    continue;

                var firstSchedule = schedules.First();
                var clinic = firstSchedule.Clinic;

                result.Add(new DoctorAvailabilityDTO
                {
                    Id = doc.Id,
                    Name = doc.User?.FullName ?? string.Empty,
                    SpecialtyName = doc.Specialty?.Name ?? string.Empty,
                    ClinicId = clinic?.Id,
                    ClinicRoom = clinic?.RoomNumber ?? "Chưa sắp phòng",
                    ClinicFloor = clinic?.Floor,
                    TimeSlots = timeSlots
                });
            }

            return result;
        }
    }
}
