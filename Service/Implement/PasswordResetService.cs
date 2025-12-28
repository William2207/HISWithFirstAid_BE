using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace FirstAidAPI.Service.Implement
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PasswordResetService> _logger;
        private const int OTP_LENGTH = 6;
        private const int OTP_EXPIRY_MINUTES = 15;

        public PasswordResetService(
            IUserRepository userRepository,
            IPasswordResetTokenRepository tokenRepository,
            IEmailService emailService,
            UserManager<User> userManager,
            ILogger<PasswordResetService> logger)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> SendPasswordResetOtpAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Empty email provided for password reset");
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                // Don't reveal if email exists in the system for security
                return true;
            }

            // Xóa các OTP cũ chưa sử dụng
            await _tokenRepository.DeleteUnusedTokensAsync(user.Id);

            // Tạo OTP mới
            var otp = GenerateOtp();
            var passwordResetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES),
                IsUsed = false
            };

            await _tokenRepository.AddAsync(passwordResetToken);
            await _tokenRepository.SaveChangesAsync();

            _logger.LogInformation("Password reset OTP generated for user: {UserId}", user.Id);

            // Gửi email chứa OTP
            var emailBody = GetPasswordResetEmailTemplate(user.FullName, otp);
            await _emailService.SendEmailAsync(user.Email!, "Mã OTP đặt lại mật khẩu", emailBody);

            return true;
        }

        public async Task<string> VerifyOtpAndResetPasswordAsync(string email, string otp)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
            {
                throw new ArgumentException("Email và OTP không được để trống");
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException("Người dùng không tồn tại");
            }

            // Xác thực OTP
            var isValid = await ValidateOtpAsync(email, otp);
            if (!isValid)
            {
                throw new ArgumentException("OTP không hợp lệ hoặc đã hết hạn");
            }

            // Đánh dấu token là đã sử dụng
            var token = await _tokenRepository.GetValidTokenByUserIdAndOtpAsync(user.Id, otp);
            if (token != null)
            {
                token.IsUsed = true;
                _tokenRepository.Update(token);
                await _tokenRepository.SaveChangesAsync();
            }

            // Tạo mật khẩu mới
            var newPassword = GenerateRandomPassword();

            // Cập nhật mật khẩu
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                throw new InvalidOperationException("Không thể xóa mật khẩu cũ");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addPasswordResult.Succeeded)
            {
                throw new InvalidOperationException("Không thể đặt mật khẩu mới");
            }

            _logger.LogInformation("Password reset successfully for user: {UserId}", user.Id);

            // Gửi email chứa mật khẩu mới
            var emailBody = GetNewPasswordEmailTemplate(user.FullName, newPassword);
            await _emailService.SendEmailAsync(user.Email!, "Mật khẩu mới của bạn", emailBody);

            return newPassword;
        }

        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
            {
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var token = await _tokenRepository.GetValidTokenByUserIdAndOtpAsync(user.Id, otp);

            if (token == null)
            {
                _logger.LogWarning("Invalid or expired OTP for user: {UserId}", user.Id);
                return false;
            }

            if (token.IsUsed)
            {
                _logger.LogWarning("OTP already used for user: {UserId}", user.Id);
                return false;
            }

            if (token.ExpirationTime < DateTime.UtcNow)
            {
                _logger.LogWarning("OTP expired for user: {UserId}", user.Id);
                return false;
            }

            return true;
        }

        private string GenerateOtp()
        {
            var random = new Random();
            var otp = string.Empty;
            for (int i = 0; i < OTP_LENGTH; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            return otp;
        }

        private string GenerateRandomPassword()
        {
            var length = 12;
            var charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            var stringBuilder = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(charset[random.Next(charset.Length)]);
            }

            return stringBuilder.ToString();
        }

        private string GetPasswordResetEmailTemplate(string userName, string otp)
        {
            return $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ background-color: #f5f5f5; padding: 20px; }}
                    .otp {{ background-color: white; padding: 20px; margin: 20px 0; text-align: center; border-radius: 5px; }}
                    .otp-code {{ font-size: 32px; font-weight: bold; color: #2196F3; letter-spacing: 5px; }}
                    .expiry {{ color: #f44336; font-size: 14px; margin-top: 10px; }}
                    .footer {{ background-color: #f5f5f5; padding: 20px; text-align: center; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>Đặt lại mật khẩu</h1>
                    </div>
                    <div class=""content"">
                        <p>Xin chào <strong>{userName}</strong>,</p>
                        <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản của mình. Vui lòng sử dụng mã OTP dưới đây để xác nhận yêu cầu:</p>

                        <div class=""otp"">
                            <div class=""otp-code"">{otp}</div>
                            <div class=""expiry"">Mã này sẽ hết hạn sau 15 phút</div>
                        </div>

                        <p><strong>Lưu ý:</strong> Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này và kiểm tra tài khoản của bạn.</p>
                        <p>Đây là email tự động, vui lòng không trả lời email này.</p>
                    </div>
                    <div class=""footer"">
                        <p>&copy; 2024 FirstAid. Tất cả quyền được bảo lưu.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        private string GetNewPasswordEmailTemplate(string userName, string newPassword)
        {
            return $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ background-color: #f5f5f5; padding: 20px; }}
                    .password {{ background-color: white; padding: 20px; margin: 20px 0; text-align: center; border-radius: 5px; border-left: 4px solid #4CAF50; }}
                    .password-text {{ font-size: 18px; font-family: monospace; color: #333; background-color: #f0f0f0; padding: 10px; border-radius: 3px; }}
                    .warning {{ background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; margin: 20px 0; border-radius: 5px; color: #856404; }}
                    .footer {{ background-color: #f5f5f5; padding: 20px; text-align: center; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>Mật khẩu đã được đặt lại thành công</h1>
                    </div>
                    <div class=""content"">
                        <p>Xin chào <strong>{userName}</strong>,</p>
                        <p>Mật khẩu của bạn đã được đặt lại thành công. Dưới đây là mật khẩu tạm thời của bạn:</p>

                        <div class=""password"">
                            <p>Mật khẩu tạm thời:</p>
                            <div class=""password-text"">{newPassword}</div>
                        </div>

                        <div class=""warning"">
                            <p><strong>⚠️ Quan trọng:</strong></p>
                            <ul>
                                <li>Vui lòng sao chép mật khẩu này và giữ nó ở nơi an toàn</li>
                                <li>Đăng nhập vào tài khoản của bạn bằng mật khẩu này</li>
                                <li>Thay đổi mật khẩu thành một mật khẩu riêng của bạn ngay sau khi đăng nhập</li>
                                <li>Không chia sẻ mật khẩu này với bất kỳ ai</li>
                            </ul>
                        </div>

                        <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng liên hệ với chúng tôi ngay lập tức.</p>
                        <p>Đây là email tự động, vui lòng không trả lời email này.</p>
                    </div>
                    <div class=""footer"">
                        <p>&copy; 2024 FirstAid. Tất cả quyền được bảo lưu.</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
