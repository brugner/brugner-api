using System;
using Brugner.API.Core.Models.DTOs.Posts;

namespace Brugner.API.Core.Contracts.Services
{
    public interface IPostsService
    {
        /// <summary>
        /// Get all posts.
        /// </summary>
        /// <param name="includeDrafts">True to include posts marked as draft.</param>
        /// <param name="tag">If specified it will return only posts that contain this tag.</param>
        /// <returns></returns>
        Task<IEnumerable<PostDTO>> GetAllAsync(bool includeDrafts = false, string? tag = null);

        /// <summary>
        /// Get a post with the specified slug.
        /// </summary>
        /// <param name="slug">Slug.</param>
        /// <returns></returns>
        Task<PostDTO> GetBySlugAsync(string slug);

        /// <summary>
        /// Get a post.
        /// </summary>
        /// <param name="id">Post Id.</param>
        /// <returns></returns>
        Task<PostDTO> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="postForCreation">Post data.</param>
        /// <returns></returns>
        Task<PostDTO> CreateAsync(PostForCreationDTO postForCreation);

        /// <summary>
        /// Updates the specified post.
        /// </summary>
        /// <param name="postForUpdate">Post data.</param>
        /// <returns></returns>
        Task<PostDTO> UpdateAsync(PostForUpdateDTO postForUpdate);

        /// <summary>
        /// Deletes the specified post.
        /// </summary>
        /// <param name="id">Post Id.</param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetAllTagsAsync();
    }
}

