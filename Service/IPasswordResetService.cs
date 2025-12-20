namespace FirstAidAPI.Service
{
    public interface IPasswordResetService
    {
        Task<bool> SendPasswordResetOtpAsync(string email);

        Task<string> VerifyOtpAndResetPasswordAsync(string email, string otp);

        Task<bool> ValidateOtpAsync(string email, string otp);
    }
}
