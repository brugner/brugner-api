using System;
using System.Net.Http.Headers;
using Brugner.API.Core.Contracts.Services;

namespace Brugner.API.Core.Services
{
    public class FilesService : IFilesService
    {
        private readonly string _postsFolder = Path.Combine("images", "posts");
        private const string THUMBNAIL = "thumbnail";
        private readonly ILogger<FilesService> _logger;

        public FilesService(ILogger<FilesService> logger)
        {
            _logger = logger;
        }

        public async Task<string?> SavePostThumbnailAsync(int postId, IFormFile? thumbnail)
        {
            try
            {
                if (thumbnail == null || thumbnail.Length == 0 || postId <= 0)
                {
                    return null;
                }

                var pathToFolder = GetPathToPostsFolder(postId);
                var fileName = ContentDispositionHeaderValue.Parse(thumbnail.ContentDisposition).FileName?.Trim('"');

                fileName = THUMBNAIL + Path.GetExtension(fileName);
                fileName = fileName.ToLower();

                var pathToFile = Path.Combine(pathToFolder, fileName);

                DeletePostThumbnail(postId);

                if (!Directory.Exists(pathToFolder))
                {
                    Directory.CreateDirectory(pathToFolder);
                }

                using var stream = new FileStream(pathToFile, FileMode.Create);
                await thumbnail.CopyToAsync(stream);

                return Path.Combine(_postsFolder, postId.ToString(), fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public void DeletePostThumbnail(int postId)
        {
            var pathToFolder = GetPathToPostsFolder(postId);

            if (!Directory.Exists(pathToFolder))
            {
                return;
            }

            var existingFiles = Directory.GetFiles(pathToFolder, $"{THUMBNAIL}.*");

            if (existingFiles.Length > 0)
            {
                foreach (var file in existingFiles)
                {
                    File.Delete(file);
                }
            }

            Directory.Delete(pathToFolder);
        }

        private string GetPathToPostsFolder(int postId)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _postsFolder, postId.ToString());
        }
    }
}

