using Brugner.API.Core.Constants;
using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Exceptions;
using Brugner.API.Core.Models.DTOs.Auth;
using Brugner.API.Core.Models.Entities;
using Brugner.API.Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Brugner.API.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHashService _hashService;
        private readonly JwtOptions _jwtOptions;

        public AuthService(IUsersRepository usersRepository, IHashService hashService, IOptionsMonitor<JwtOptions> jwtOptions)
        {
            _usersRepository = usersRepository;
            _hashService = hashService;
            _jwtOptions = jwtOptions.CurrentValue;
        }

        public async Task<AuthResultDTO> LoginAsync(UserForAuthDTO userForAuth)
        {
            var user = await _usersRepository.GetByEmailAsync(userForAuth.Email);

            if (user is null || !IsValidAttempt(user, userForAuth.Password))
            {
                throw new InvalidArgumentAPIException("Email or password invalid");
            }

            return new AuthResultDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = CreateTokenForUser(user)
            };
        }

        public async Task ChangePasswordAsync(ChangePasswordDTO changePassword, int userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new InvalidArgumentAPIException("Invalid user");
            }

            var currentPasswordIsValid = _hashService.ValidatePassword(changePassword.CurrentPassword, user.PasswordHash);

            if (!currentPasswordIsValid || !changePassword.NewPassword.Equals(changePassword.RepeatNewPassword))
            {
                throw new InvalidArgumentAPIException("Invalid password");
            }

            var newPasswordHash = _hashService.HashPassword(changePassword.NewPassword);
            user.PasswordHash = newPasswordHash;

            await _usersRepository.SaveChangesAsync();
        }

        #region Helpers
        /// <summary>
        /// Returns true if its a valid login attempt.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="plainPassword">Plain password.</param>
        /// <returns></returns>
        private bool IsValidAttempt(User user, string plainPassword)
        {
            return _hashService.ValidatePassword(plainPassword, user.PasswordHash);
        }

        /// <summary>
        /// Creates a JSON Web Token for the specified user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns></returns>
        private string CreateTokenForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(AppClaims.Id, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}

