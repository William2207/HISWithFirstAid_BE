using FirstAidAPI.Data;
using FirstAidAPI.DTO.User;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StaffController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly FirstAidContext _context;
        private readonly IUserService _userService;

        public StaffController(UserManager<User> userManager, FirstAidContext context, IUserService userService)
        {
            _userManager = userManager;
            _context = context;
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStaff()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var nurses = await _userManager.GetUsersInRoleAsync("Nurse");
            var receptionists = await _userManager.GetUsersInRoleAsync("Receptionist");

            var staffIds = doctors.Concat(nurses).Concat(receptionists).Select(u => u.Id).Distinct().ToList();

            var allStaff = await _userManager.Users
                .Include(u => u.Doctor).ThenInclude(d => d.Specialty)
                .Include(u => u.Nurse).ThenInclude(n => n.Speciality)
                .Where(u => staffIds.Contains(u.Id))
                .ToListAsync();

            var staffDtos = new List<UserDto>();
            foreach (var user in allStaff)
            {
                var roles = await _userManager.GetRolesAsync(user);
                int? specialtyId = null;
                string? specialtyName = null;
                int? doctorId = null;
                int? nurseId = null;
                bool isHeadDoctor = false;
                bool isHeadNurse = false;

                if (user.Doctor != null)
                {
                    doctorId = user.Doctor.Id;
                    specialtyId = user.Doctor.SpecialtyId;
                    specialtyName = user.Doctor.Specialty?.Name;
                    isHeadDoctor = user.Doctor.Specialty?.HeadDoctorId == user.Doctor.Id;
                }
                else if (user.Nurse != null)
                {
                    nurseId = user.Nurse.Id;
                    specialtyId = user.Nurse.SpecialityId;
                    specialtyName = user.Nurse.Speciality?.Name;
                    isHeadNurse = user.Nurse.Speciality?.HeadNurseId == user.Nurse.Id;
                }

                staffDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Avatar = user.Avatar,
                    DateOfBirth = user.DateOfBirth,
                    Role = roles,
                    SpecialtyId = specialtyId,
                    SpecialtyName = specialtyName,
                    DoctorId = doctorId,
                    NurseId = nurseId,
                    IsHeadDoctor = isHeadDoctor,
                    IsHeadNurse = isHeadNurse,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive
                });
            }

            return Ok(staffDtos);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] UpdateStaffRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users
                .Include(u => u.Doctor)
                .Include(u => u.Nurse)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy nhân viên." });
            }

            // Cập nhật thông tin cơ bản
            user.FullName = request.FullName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var roles = await _userManager.GetRolesAsync(user);

            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrEmpty(request.Password))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (removeResult.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, request.Password);
                }
                else
                {
                    return BadRequest(new { message = "Không thể cập nhật mật khẩu mới." });
                }
            }

            // Cập nhật chuyên khoa và chức vụ trưởng khoa/điều dưỡng trưởng
            if (roles.Contains("Doctor") && user.Doctor != null)
            {
                int oldSpecialtyId = user.Doctor.SpecialtyId;
                int newSpecialtyId = request.SpecialtyId ?? 0;

                if (newSpecialtyId > 0)
                {
                    // Nếu đổi chuyên khoa và đang là trưởng khoa cũ, bỏ trưởng khoa cũ
                    if (oldSpecialtyId != newSpecialtyId)
                    {
                        var oldSpecialty = await _context.Specialties.FindAsync(oldSpecialtyId);
                        if (oldSpecialty != null && oldSpecialty.HeadDoctorId == user.Doctor.Id)
                        {
                            oldSpecialty.HeadDoctorId = null;
                            _context.Specialties.Update(oldSpecialty);
                        }
                    }

                    user.Doctor.SpecialtyId = newSpecialtyId;
                    
                    // Cập nhật trạng thái Trưởng khoa mới
                    var newSpecialty = await _context.Specialties.FindAsync(newSpecialtyId);
                    if (newSpecialty != null)
                    {
                        if (request.IsHead)
                        {
                            newSpecialty.HeadDoctorId = user.Doctor.Id;
                        }
                        else if (newSpecialty.HeadDoctorId == user.Doctor.Id)
                        {
                            newSpecialty.HeadDoctorId = null;
                        }
                        _context.Specialties.Update(newSpecialty);
                    }
                }
            }
            else if (roles.Contains("Nurse") && user.Nurse != null)
            {
                int oldSpecialtyId = user.Nurse.SpecialityId;
                int newSpecialtyId = request.SpecialtyId ?? 0;

                if (newSpecialtyId > 0)
                {
                    // Nếu đổi chuyên khoa và đang là điều dưỡng trưởng cũ, bỏ cũ
                    if (oldSpecialtyId != newSpecialtyId)
                    {
                        var oldSpecialty = await _context.Specialties.FindAsync(oldSpecialtyId);
                        if (oldSpecialty != null && oldSpecialty.HeadNurseId == user.Nurse.Id)
                        {
                            oldSpecialty.HeadNurseId = null;
                            _context.Specialties.Update(oldSpecialty);
                        }
                    }

                    user.Nurse.SpecialityId = newSpecialtyId;

                    // Cập nhật trạng thái Điều dưỡng trưởng mới
                    var newSpecialty = await _context.Specialties.FindAsync(newSpecialtyId);
                    if (newSpecialty != null)
                    {
                        if (request.IsHead)
                        {
                            newSpecialty.HeadNurseId = user.Nurse.Id;
                        }
                        else if (newSpecialty.HeadNurseId == user.Nurse.Id)
                        {
                            newSpecialty.HeadNurseId = null;
                        }
                        _context.Specialties.Update(newSpecialty);
                    }
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(new { message = string.Join(", ", updateResult.Errors.Select(e => e.Description)) });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật tài khoản nhân viên thành công!" });
        }

        [HttpPut("{id:int}/toggle-status")]
        public async Task<IActionResult> ToggleStaffStatus(int id)
        {
            var result = await _userService.ToggleUserStatusAsync(id);

            if (!result.Success)
            {
                if (result.Message.Contains("tìm thấy"))
                {
                    return NotFound(new { message = result.Message });
                }
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message, isActive = result.IsActive });
        }
    }
}
