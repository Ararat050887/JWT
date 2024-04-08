using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_API_SECOND_EXPIRIENCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger) 
        {
            _logger = logger;
        }

        [HttpPost("GetData")]
        public IActionResult GetData()
        {
            try
            {
                _logger.LogInformation("This is AdminController");

            return Ok("Data from admin controller");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while refreshing token: " + ex.Message);
            }
}
    }
}
