using FirstAidAPI.DTO.Nurse;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.AspNetCore.Identity;

namespace FirstAidAPI.Service.Implement
{
    public class NurseService : INurseService
    {
        private readonly INurseRepository _nurseRepository;
        private readonly UserManager<User> _userManager;

        public NurseService(INurseRepository nurseRepository, UserManager<User> userManager)
        {
            _nurseRepository = nurseRepository;
            _userManager = userManager;
        }

        public async Task<NurseProfileDto?> GetNurseProfileAsync(int userId)
        {
            var nurse = await _nurseRepository.GetByUserIdAsync(userId);
            if (nurse == null) return null;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return new NurseProfileDto
            {
                Id = nurse.Id,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                IdCard = user.IdCard,
                SpecialtyId = nurse.SpecialityId,
                SpecialtyName = nurse.Speciality?.Name ?? string.Empty,
                LicenseNumber = nurse.LicenseNumber,
                Qualifications = nurse.Qualifications,
                YearsOfExperience = nurse.YearsOfExperience,
                IsHeadNurse = nurse.Speciality?.HeadNurseId == nurse.Id
            };
        }

        public async Task<bool> UpdateNurseProfileAsync(int userId, UpdateNurseProfileDto updateDto)
        {
            // Update User
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

            // Update Nurse
            var nurse = await _nurseRepository.GetByUserIdAsync(userId);
            if (nurse != null)
            {
                if (updateDto.LicenseNumber != null) nurse.LicenseNumber = updateDto.LicenseNumber;
                if (updateDto.Qualifications != null) nurse.Qualifications = updateDto.Qualifications;
                if (updateDto.YearsOfExperience.HasValue) nurse.YearsOfExperience = updateDto.YearsOfExperience.Value;

                await _nurseRepository.UpdateAsync(nurse);
            }

            return true;
        }

        public async Task<int> GetNurseIdByUserId(int userId)
        {
            var nurse = await _nurseRepository.GetByUserIdAsync(userId);
            if (nurse == null) throw new Exception("Nurse not found for user ID: " + userId);
            return nurse.Id;
        }
    }
}
