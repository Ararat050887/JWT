using Azure.Core;
using JWT_API_SECOND_EXPIRIENCE.Models.Domain;
using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using JWT_API_SECOND_EXPIRIENCE.Repositories.Abstract;
using JWT_API_SECOND_EXPIRIENCE.Repositories.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_API_SECOND_EXPIRIENCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly DatabaseContext _ctx;
        private readonly ITokenService _service;

        public TokenController(ILogger<TokenController> logger , DatabaseContext ctx, ITokenService service)
        {
            _logger = logger;
            _ctx = ctx;
            _service = service;
        }
        [HttpPost("GetData")]
        public IActionResult GetData()
        {
            _logger.LogInformation("This is TokenController");

            return Ok("Ok");
        }

        //private readonly DatabaseContext _ctx;
        //private readonly ITokenService _service;
        //public TokenController(DatabaseContext ctx, ITokenService service)
        //{
        //        _ctx = ctx;
        //    _service = service;
        //}

        [HttpPost("Refresh")]
        public IActionResult Refresh(RefreshTokenRequest tokenApiModel)
        {
            try
            {
                if (tokenApiModel is null)
                    return BadRequest("Invalid client request");
                string accessToken = tokenApiModel.AccessToken;
                string refreshToken = tokenApiModel.RefreshToken;
                var principal = _service.GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Identity.Name;
                var user = _ctx.TokenInfo.SingleOrDefault(u => u.Usename == username);
                if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                    return BadRequest("Invalid client request");
                var newAccessToken = _service.GetToken(principal.Claims);
                var newRefreshToken = _service.GetRefreshToken();
                user.RefreshToken = newRefreshToken;
                _ctx.SaveChanges();
                return Ok(new RefreshTokenRequest()
                {
                    AccessToken = newAccessToken.TokenString,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while refreshing token: " + ex.Message);
            }
        }

        //revoken is use for removing token enntry
        [HttpPost("Revoke"), Authorize]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.Identity.Name;
                var user = _ctx.TokenInfo.SingleOrDefault(u => u.Usename == username);
                if (user is null)
                    return BadRequest();
                user.RefreshToken = null;
                _ctx.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


    }
}
//try
//{
//    if (tokenApiModel is null)
//        return BadRequest("Invalid client request");
//    string accessToken = tokenApiModel.AccessToken;
//    string refreshToken = tokenApiModel.RefreshToken;
//    var principal = _service.GetPrincipalFromExpiredToken(accessToken);
//    var username = principal.Identity.Name;
//    var user = _ctx.TokenInfo.SingleOrDefault(u => u.Usename == username);
//    if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
//        return BadRequest("Invalid client request");
//    var newAccessToken = _service.GetToken(principal.Claims);
//    var newRefreshToken = _service.GetRefreshToken();
//    user.RefreshToken = newRefreshToken;
//    _ctx.SaveChanges();
//    return Ok(new RefreshTokenRequest()
//    {
//        AccessToken = newAccessToken.TokenString,
//        RefreshToken = newRefreshToken
//    });
//}
//catch (Exception ex)
//{
//    return BadRequest("An error occurred while refreshing token: " + ex.Message);
//}