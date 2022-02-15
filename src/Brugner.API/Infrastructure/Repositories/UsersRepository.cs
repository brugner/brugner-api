using System;
using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brugner.API.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly BrugnerDbContext _dbContext;

        public UsersRepository(BrugnerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

