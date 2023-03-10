using Bogus;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Extensions;
using Brugner.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brugner.API.Infrastructure.Services
{
    public class DbManagerService : IDbManagerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DbManagerService> _logger;

        public DbManagerService(IServiceScopeFactory scopeFactory, ILogger<DbManagerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            Randomizer.Seed = new Random(08012022);
        }

        public void Migrate()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<BrugnerDbContext>();

            if (context is null)
                throw new Exception("DbContext is null");

            foreach (var item in context.Database.GetPendingMigrations())
            {
                _logger.LogInformation("Applying pending migration: {item}", item);
            }

            context.Database.Migrate();
        }

        public void Seed()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<BrugnerDbContext>();

            if (context is null)
                throw new Exception("DbContext is null");

            SeedUsers(context);
            SeedPosts(context);
        }

        private void SeedUsers(BrugnerDbContext context)
        {
            if (!context.Users.Any())
            {
                _logger.LogInformation("Seeding users..");

                var users = GetUserSeed();

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private void SeedPosts(BrugnerDbContext context)
        {
            if (!context.Posts.Any())
            {
                _logger.LogInformation("Seeding posts..");

                var posts = GetPostsSeeds();

                context.Posts.AddRange(posts);
                context.SaveChanges();
            }
        }

        private static User GetUserSeed()
        {
            return new User()
            {
                Email = "editor1@brugner.com",
                PasswordHash = "1000:RIGtm0DVzuZ4dEtiQncCm4LfujPS9iud:utqaw7AM6GNd4VcbGrplshEbAlE=",
                FirstName = "Editor",
                LastName = "One"
            };
        }

        private static Post[] GetPostsSeeds()
        {
            var tags = new[] { "software", "short-story", "music", "movies", "stuff", "lyrics", "writing", "sci-fi", "thought" };

            var title = string.Empty;

            var posts = new Faker<Post>()
                .RuleFor(x => x.Title, faker =>
                {
                    title = faker.Commerce.ProductAdjective() + " " + faker.Commerce.ProductName();
                    return title;
                })
                .RuleFor(x => x.Slug, faker => title.ToSlug())
                .RuleFor(x => x.Summary, faker =>
                {
                    var summary = faker.Lorem.Paragraph();

                    return summary.Length <= 200 ? summary : summary[..199];
                })
                .RuleFor(x => x.Content, faker => faker.Lorem.Paragraphs(5))
                .RuleFor(x => x.Tags, faker => string.Join(',', faker.Random.ArrayElements(tags, faker.Random.Int(1, 3))))
                .RuleFor(x => x.IsDraft, faker => faker.Random.Bool(0.25F))
                .RuleFor(x => x.CreatedAt, faker => faker.Date.PastOffset())
                .RuleFor(x => x.UpdatedAt, faker => faker.Date.PastOffset())
                .Generate(25);

            return posts.ToArray();
        }
    }
}

