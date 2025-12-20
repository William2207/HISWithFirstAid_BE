using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly FirstAidContext _context;

        public PasswordResetTokenRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
        }

        public void Update(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Update(token);
        }

        public async Task<PasswordResetToken?> GetValidTokenByUserIdAndOtpAsync(int userId, string otp)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && 
                                          t.Otp == otp && 
                                          !t.IsUsed && 
                                          t.ExpirationTime > DateTime.UtcNow);
        }

        public async Task DeleteUnusedTokensAsync(int userId)
        {
            var oldTokens = await _context.PasswordResetTokens
                .Where(t => t.UserId == userId && !t.IsUsed)
                .ToListAsync();

            _context.PasswordResetTokens.RemoveRange(oldTokens);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
