using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWT_API_SECOND_EXPIRIENCE.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
  
    }
}
