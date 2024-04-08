using JWT_API_SECOND_EXPIRIENCE.Models;
using JWT_API_SECOND_EXPIRIENCE.Models.Domain;
using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using JWT_API_SECOND_EXPIRIENCE.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;

namespace JWT_API_SECOND_EXPIRIENCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {        
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService _tokenService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;
        public AuthorizationController(DatabaseContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IAuthorizationService authorizationService,
            ILogger<AuthorizationController> logger
            )
        {
            this._context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._tokenService = tokenService;
            this._authorizationService = authorizationService;
            this._logger = logger;

        }

        [HttpPost("GetLog")]
        public IActionResult GetLog()
        {
            try
            {

                _logger.LogInformation("This is AuthorizationController");
                return Ok("Ok");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while refreshing token: " + ex.Message);
            }

        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var status = new Status();
                // check validation
                if (!ModelState.IsValid)
                {
                    status.StatusCode = 0;
                    status.Message = "Please pass all valid fields";
                    return Ok(status);
                }
                // lets find the user
                var user = await userManager.FindByNameAsync(model.Username);
                if (user is null)
                {
                    status.StatusCode = 0;
                    status.Message = "Invalid username";
                    return Ok(status);
                }
                // check current password
                if (!await userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {
                    status.StatusCode = 0;
                    status.Message = "Invalid current password";
                    return Ok(status);
                }

                // change password here
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    status.StatusCode = 0;
                    status.Message = "Failed to change password";
                    return Ok(status);
                }
                status.StatusCode = 1;
                status.Message = "Password has changed successfully";
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while refreshing token: " + ex.Message);
            }
        }

        

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            try
            {
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var accessToken = _tokenService.GetToken(authClaims);
                    var refreshToken = _tokenService.GetRefreshToken();
                    var tokenInfo = _context.TokenInfo.FirstOrDefault(a => a.Usename == user.UserName);
                    if (tokenInfo == null)
                    {
                        var info = new TokenInfo
                        {
                            Usename = user.UserName,
                            RefreshToken = refreshToken,
                            RefreshTokenExpiry = DateTime.Now.AddDays(7),
                        };
                        _context.TokenInfo.Add(info);
                    }
                    else
                    {
                        tokenInfo.RefreshToken = refreshToken;
                        tokenInfo.RefreshTokenExpiry = DateTime.Now.AddDays(7);
                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                    HttpContext.Response.Cookies.Append("jwt", refreshToken,
                       new CookieOptions
                       {
                           MaxAge = TimeSpan.FromMinutes(1440)
                       });

                    return Ok(new LoginResponse
                    {
                        Name = user.Name,
                        Username = user.UserName,
                        AccessToken = accessToken.TokenString,
                        RefreshToken = refreshToken,
                        Expiration = accessToken.ValidTo,
                        StatusCode = 1,
                        Message = "Logged in",
                        Roles = userRoles.ToList()

                    });
                }
                //login failed condition

                return Ok(
                    new LoginResponse
                    {
                        StatusCode = 0,
                        Message = "Invalid Username or Password",
                        AccessToken = "",
                        Expiration = null,
                    });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
            
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel model)
        {
            var status = new Status();
            try
            {
                if (!ModelState.IsValid)
                {
                    status.StatusCode = 0;
                    status.Message = "Please pass all the required fields";
                    return Ok(status);
                }
                // check if user exists
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    status.StatusCode = 0;
                    status.Message = "Invalid username. Username already exists.";
                    return Ok(status);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    Name = model.Name,
                };
                // create a user here
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {

                    if (model.Password.Length < 8 || model.Password.Length > 30)
                    {
                        status.Message = "Password must be minimum 8 digits maximum 30 digits.";
                        return Ok(status);
                    }

                        
                    if (!Regex.IsMatch(model.Password, @"[A-Z]"))
                    {
                        status.Message = "Include in password at least one upper case character.";
                        return Ok(status);
                    }

                    if (!Regex.IsMatch(model.Password, @"[a-z]"))
                    {
                        status.Message = "Include in password at least one lower case character.";
                        return Ok(status);
                    }

                    if (!Regex.IsMatch(model.Password, @"\d"))
                    {
                        status.Message = "Include in password at least one digit.";
                        return Ok(status);
                    }

                    if (!Regex.IsMatch(model.Password, @"[!@#$%^&*]"))
                    {
                        status.Message = "Include in password at least one special character.";
                        return Ok(status);
                    }
                    
                    status.StatusCode = 0;
                    status.Message = "User creation failed";
                    return Ok(status);
                }


                if (model.Roles != null)
                {
                    foreach (var role in model.Roles)
                    {
                        if (role == UserRoles.Admin && await roleManager.RoleExistsAsync(UserRoles.Admin))
                        {
                            await userManager.AddToRoleAsync(user, UserRoles.Admin);
                        }

                        if (role == UserRoles.Editor && await roleManager.RoleExistsAsync(UserRoles.Editor))
                        {
                            await userManager.AddToRoleAsync(user, UserRoles.Editor);
                        }

                        if (role == UserRoles.Moderator && await roleManager.RoleExistsAsync(UserRoles.Moderator))
                        {
                            await userManager.AddToRoleAsync(user, UserRoles.Moderator);
                        }
                    }
                }


                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }

                if (await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }



                status.StatusCode = 1;
                status.Message = "Successfully registered";
                return Ok(status);
            }
            catch(Exception ex)
            {
                status.StatusCode = 0;
                status.Message = "An error occurred: " + ex.Message;
                 return Ok(status);
            }
        }

       

    }
}



        //after registering admin we will comment  this code, because we want only one admin in this application
        //[HttpPost]
        //public async Task<IActionResult> RegistrationAdmin([FromBody] RegistrationModel model)
        //{
        //    var status = new Status();
        //    if (!ModelState.IsValid)
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "Please pass all the required fields";
        //        return Ok(status);
        //    }
        //    // check if user exists
        //    var userExists = await userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "Invalid username";
        //        return Ok(status);
        //    }
        //    Console.WriteLine(userExists);
        //    var user = new ApplicationUser
        //    {
        //        UserName = model.Username,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        Email = model.Email,
        //        Name = model.Name,
        //    };
        //    // create a user here
        //    var result = await userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "User creation failed";
        //        return Ok(status);
        //    }

        //    // add roles here
        //    // for admin registration UserRoles.Admin instead of UserRoles.Roles
        //    if (!await roleManager.RoleExistsAsync(UserRoles.Moderator))
        //        await roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));

        //    if (await roleManager.RoleExistsAsync(UserRoles.Moderator))
        //    {
        //        await userManager.AddToRoleAsync(user, UserRoles.Moderator);
        //    }

        //    status.StatusCode = 1;
        //    status.Message = "Successfully registered";
        //    return Ok(status);
        //}



// add roles here
// for admin registration UserRoles.Admin instead of UserRoles.Roles(Maybe he means instead of UserRoles.User)
//if (!await roleManager.RoleExistsAsync(UserRoles.User))
//    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

//if (!await roleManager.RoleExistsAsync(UserRoles.User))
//{
//    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
//}

//if (await roleManager.RoleExistsAsync(UserRoles.User))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.User);
//}

//if (model.Roles.Contains(UserRoles.Admin) && await roleManager.RoleExistsAsync(UserRoles.Admin))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.Admin);
//}

//if (model.Roles.Contains(UserRoles.Editor) && await roleManager.RoleExistsAsync(UserRoles.Editor))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.Editor);
//}

//if (model.Roles.Contains(UserRoles.Moderator) && await roleManager.RoleExistsAsync(UserRoles.Moderator))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.Moderator);
//}


//if (model.Roles.Contains(UserRoles.Admin) && await roleManager.RoleExistsAsync(UserRoles.Admin))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.Admin);
//}

//if (model.Roles.Contains(UserRoles.Editor) && await roleManager.RoleExistsAsync(UserRoles.Editor))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.Editor);
//}

//if (model.Roles.Contains(UserRoles.Moderator) && await roleManager.RoleExistsAsync(UserRoles.Moderator))
//{
//    await userManager.AddToRoleAsync(user, UserRoles.Moderator);
//}