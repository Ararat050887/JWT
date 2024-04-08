using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]   
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [NotMapped]
        public List<string>? Roles { get; set; }
    }
}
