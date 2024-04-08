using System.ComponentModel.DataAnnotations;

namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
