using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brugner.API.Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly BrugnerDbContext _dbContext;

        public PostsRepository(BrugnerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Post>> GetAllAsync(bool includeDrafts = false, string? tag = null)
        {
            var postsQuery = _dbContext.Posts.AsQueryable();

            if (!includeDrafts)
            {
                postsQuery = postsQuery.Where(x => !x.IsDraft);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                tag = tag.ToLower();
                postsQuery = postsQuery.Where(x => x.Tags.Contains(tag));
            }

            var posts = await postsQuery.ToListAsync();

            return posts.OrderByDescending(x => x.CreatedAt);
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _dbContext.Posts.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Post?> GetBySlugAsync(string slug)
        {
            return await _dbContext.Posts.SingleOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<IEnumerable<string>> GetAllTagsAsync()
        {
            var rawTags = await _dbContext
                .Posts.Where(x => !x.IsDraft)
                .Select(x => x.Tags)
                .ToListAsync();

            return rawTags
                .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Distinct()
                .OrderBy(x => x);
        }

        public async Task AddAsync(Post post)
        {
            await _dbContext.AddAsync(post);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Remove(Post post)
        {
            _dbContext.Remove(post);
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            var exists = await _dbContext.Posts.AnyAsync(x => x.Slug == slug);

            return exists;
        }
    }
}

