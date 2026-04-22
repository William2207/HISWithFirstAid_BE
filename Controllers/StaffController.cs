using FirstAidAPI.DTO.User;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public StaffController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStaff()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var nurses = await _userManager.GetUsersInRoleAsync("Nurse");
            var receptionists = await _userManager.GetUsersInRoleAsync("Receptionist");

            var allStaff = doctors.Concat(nurses).Concat(receptionists).DistinctBy(u => u.Id).ToList();

            var staffDtos = new List<UserDto>();
            foreach (var user in allStaff)
            {
                var roles = await _userManager.GetRolesAsync(user);
                staffDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Avatar = user.Avatar,
                    DateOfBirth = user.DateOfBirth,
                    Role = roles,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                });
            }

            return Ok(staffDtos);
        }
    }
}
