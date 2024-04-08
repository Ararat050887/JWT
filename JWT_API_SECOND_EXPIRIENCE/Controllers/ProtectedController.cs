using JWT_API_SECOND_EXPIRIENCE.Controllers;
using JWT_API_SECOND_EXPIRIENCE.Models.Domain;
using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using JWT_API_SECOND_EXPIRIENCE.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;

namespace JWT_API_SECOND_EXPIRIENCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProtectedController : ControllerBase
    {
        private readonly ILogger<ProtectedController> _logger;

        public ProtectedController(ILogger<ProtectedController> logger)
        {
            _logger = logger;
        }

        [HttpPost("GetData")]
        public IActionResult GetData()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Decode the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            var roles = decodedToken.Claims
             .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
             .Select(c => c.Value)
             .ToList();

            var permissions = new List<string>();

            foreach (var role in roles)
            {
                if (role == "Editor")
                {
                    permissions.Add("Editor Permission");
                }
                if (role == "Admin")
                {
                    permissions.Add("Admin Permission");
                }
                if (role == "Moderator")
                {
                    permissions.Add("Moderator Permission");
                }
            }

            permissions.Add("User Permission");

            _logger.LogInformation("This is ProtectedController");
            return Ok(permissions);
                //return Ok(new { Roles = roles });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving data: " + ex.Message);
            }
        }
    }
}
//private readonly DatabaseContext _context;
//private readonly UserManager<ApplicationUser> userManager;
//private readonly RoleManager<IdentityRole> roleManager;
//private readonly ITokenService _tokenService;

//public ProtectedController(DatabaseContext context,
//    UserManager<ApplicationUser> userManager,
//    RoleManager<IdentityRole> roleManager,
//    ITokenService tokenService
//    )
//{
//    this._context = context;
//    this.userManager = userManager;
//    this.roleManager = roleManager;
//    this._tokenService = tokenService;
//}

//[HttpPost]

//public async Task<IActionResult> PermissionLevel([FromBody] RegistrationModel model)
//{
//    var user = await userManager.FindByNameAsync(model.Username);

//    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
//    {
//        var roles = model.Roles;
//        foreach (var role in roles)
//        {
//            if (role == "Editor")
//            {
//                return Ok("Editor Permission");
//            }
//            if (role == "Admin")
//            {
//                return Ok("Admin Permission");
//            }
//            if (role == "Moderator")
//            {
//                return Ok("Moderator Permission");
//            }
//        }
//        return Ok("User permission");
//    }

//    return Ok("____________");
//}


