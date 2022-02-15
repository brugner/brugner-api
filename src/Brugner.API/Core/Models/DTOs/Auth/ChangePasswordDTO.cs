using System;
using System.ComponentModel.DataAnnotations;

namespace Brugner.API.Core.Models.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = default!;

        [Required]
        public string NewPassword { get; set; } = default!;

        [Required]
        public string RepeatNewPassword { get; set; } = default!;
    }
}

