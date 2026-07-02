using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<SavedTechnique>> GetSavedTechniquesByUserIdAsync(int userId)
        {
            return await _userRepository.GetSavedTechniquesAsync(userId);
        }

        public async Task<IEnumerable<ScenarioAttempt>> GetScenarioAttemptsByUserIdAsync(int userId, int limit)
        {
            return await _userRepository.GetScenarioAttemptsAsync(userId, limit);
        }

        public async Task<IEnumerable<UserScenarioProgress>> GetScenarioProgressesByUserIdAsync(int userId)
        {
            return await _userRepository.GetScenarioProgressesAsync(userId);
        }

        public async Task<IEnumerable<UserTechniqueProgress>> GetTechniqueProgressesByUserIdAsync(int userId)
        {
            return await _userRepository.GetTechniqueProgressesAsync(userId);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // --- Business Logic quan trọng ---
            // 1. Kiểm tra xem email đã tồn tại chưa
            if (user.Email == null)
            {
                throw new ArgumentNullException(nameof(user.Email), "User email cannot be null");
            }
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new ArgumentNullException(nameof(existingUser), "User cannot be null");
            }

            // 2. Logic băm mật khẩu (Password Hashing) NÊN được thực hiện ở đây hoặc trước khi gọi service này
            // Ví dụ: user.PasswordHash = _passwordHasher.Hash(user.Password);
            // Tuyệt đối không lưu mật khẩu dạng plain text.

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }

            // Cập nhật các trường thông tin cho phép
            // Không nên cho phép cập nhật Email, PasswordHash, Role trực tiếp qua phương thức này
            existingUser.FullName = user.FullName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Avatar = user.Avatar;
            existingUser.DateOfBirth = user.DateOfBirth;

            _userRepository.Update(existingUser);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            // Sử dụng UserManager để thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userToDelete = await _userRepository.GetByIdAsync(id);
            if (userToDelete == null)
            {
                return false;
            }

            // Business Logic: Thay vì xóa cứng, bạn có thể chỉ cần đánh dấu là không hoạt động (soft delete)
            // userToDelete.IsActive = false;
            // _userRepository.Update(userToDelete);

            // Hoặc xóa cứng khỏi DB
            _userRepository.Delete(userToDelete);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message, bool IsActive)> ToggleUserStatusAsync(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
            {
                return (false, "Không tìm thấy nhân viên.", false);
            }

            user.IsActive = !user.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return (false, "Không thể cập nhật trạng thái nhân viên.", user.IsActive);
            }

            return (true, user.IsActive ? "Tài khoản đã được kích hoạt." : "Tài khoản đã bị vô hiệu hóa.", user.IsActive);
        }
    }
}
