using System.ComponentModel.DataAnnotations;

namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class ProtectedModel
    {
        [Required]
        public string? Username { get; set; }
    }
}
