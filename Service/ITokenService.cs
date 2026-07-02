using FirstAidAPI.Models;
using System.Security.Claims;

namespace FirstAidAPI.Service
{
    public interface ITokenService
    {
        string CreateToken(User user, IList<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
