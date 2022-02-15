using System;
using Brugner.API.Core.Models.Entities;

namespace Brugner.API.Core.Contracts.Repositories
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Get user by email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns></returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Saves the changes to the database.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}

