using System;
using Brugner.API.Core.Models.Entities;
using Brugner.API.Infrastructure.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Brugner.API.Infrastructure
{
    public class BrugnerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Post> Posts { get; set; } = default!;

        public BrugnerDbContext(DbContextOptions<BrugnerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
        }
    }
}

