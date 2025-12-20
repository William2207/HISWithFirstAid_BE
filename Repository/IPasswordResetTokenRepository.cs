using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);

        void Update(PasswordResetToken token);

        Task<PasswordResetToken?> GetValidTokenByUserIdAndOtpAsync(int userId, string otp);

        Task DeleteUnusedTokensAsync(int userId);

        Task<bool> SaveChangesAsync();
    }
}
