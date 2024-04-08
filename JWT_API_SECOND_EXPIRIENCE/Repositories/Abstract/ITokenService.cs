using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using System.Security.Claims;

namespace JWT_API_SECOND_EXPIRIENCE.Repositories.Abstract
{
    public interface ITokenService
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
