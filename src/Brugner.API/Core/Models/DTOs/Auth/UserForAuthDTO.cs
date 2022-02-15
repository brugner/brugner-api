using System;
using System.ComponentModel.DataAnnotations;

namespace Brugner.API.Core.Models.DTOs.Auth
{
    public class UserForAuthDTO
    {
        [Required]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        public UserForAuthDTO()
        {

        }

        public UserForAuthDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}

