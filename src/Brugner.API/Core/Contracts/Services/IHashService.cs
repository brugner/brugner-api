using System;
namespace Brugner.API.Core.Contracts.Services
{
    public interface IHashService
    {
        /// <summary>
        /// Validates a plain text password against its hashed version. Returns true if they are equivalent.
        /// </summary>
        /// <param name="plainPassword">Plaint text password.</param>
        /// <param name="hashedPassword">Hashed password.</param>
        /// <returns></returns>
        bool ValidatePassword(string plainPassword, string hashedPassword);

        /// <summary>
        /// Creates a hash of a plain text password.
        /// </summary>
        /// <param name="plainPassword">Plain text password.</param>
        /// <returns></returns>
        string HashPassword(string plainPassword);
    }
}

