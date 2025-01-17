﻿using System.ComponentModel.DataAnnotations;

namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class ChangePasswordModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set;}
    }
}
