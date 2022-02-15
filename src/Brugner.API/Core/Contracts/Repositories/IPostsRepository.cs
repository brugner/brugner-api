using System;
using Brugner.API.Core.Models.Entities;

namespace Brugner.API.Core.Contracts.Repositories
{
    public interface IPostsRepository
    {
        /// <summary>
        /// Get all posts.
        /// </summary>
        /// <param name="includeDrafts">True to include posts marked as draft.</param>
        /// <param name="tag">If specified, it will return only those containing the tag.</param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetAllAsync(bool includeDrafts = false, string? tag = null);

        /// <summary>
        /// Get post by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns></returns>
        Task<Post?> GetByIdAsync(int id);

        /// <summary>
        /// Get a post by the specified slug.
        /// </summary>
        /// <param name="slug">Slug.</param>
        /// <returns></returns>
        Task<Post?> GetBySlugAsync(string slug);

        /// <summary>
        /// Returns true if the specified slug already exists.
        /// </summary>
        /// <param name="slug">Slug.</param>
        /// <returns></returns>
        Task<bool> SlugExistsAsync(string slug);

        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetAllTagsAsync();

        /// <summary>
        /// Adds a post.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task AddAsync(Post post);

        /// <summary>
        /// Saves the changes to the database.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Removes a post from the repository.
        /// </summary>
        /// <param name="post"></param>
        void Remove(Post post);
    }
}

