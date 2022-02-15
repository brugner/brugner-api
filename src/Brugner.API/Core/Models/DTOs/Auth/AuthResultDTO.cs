using System;
namespace Brugner.API.Core.Models.DTOs.Auth
{
    public class AuthResultDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
    }
}
