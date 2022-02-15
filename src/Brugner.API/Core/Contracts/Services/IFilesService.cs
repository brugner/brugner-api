using System;

namespace Brugner.API.Core.Contracts.Services
{
    public interface IFilesService
    {
        /// <summary>
        /// Saves the post thumbnail to the file system.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <param name="thumbnail">Post thumbnail.</param>
        /// <returns>Returns the relative path to the file.</returns>
        Task<string?> SavePostThumbnailAsync(int postId, IFormFile? thumbnail);

        /// <summary>
        /// Deletes the specified post thumbnail.
        /// </summary>
        /// <param name="postId">Post Id.</param>
        /// <returns></returns>
        void DeletePostThumbnail(int postId);
    }
}

