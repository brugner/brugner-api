using AutoMapper;
using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Exceptions;
using Brugner.API.Core.Models.DTOs.Posts;
using Brugner.API.Core.Models.Entities;
using Brugner.API.Core.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Brugner.Tests.Core.Services
{
    public class PostsServiceTests
    {
        private readonly PostsService _postsService;
        private readonly Mock<IPostsRepository> _mockPostsRepository;
        private readonly Mock<IFilesService> _mockFilesService;
        private readonly Mock<IMapper> _mockMapper;

        public PostsServiceTests()
        {
            _mockPostsRepository = new Mock<IPostsRepository>();
            _mockFilesService = new Mock<IFilesService>();
            _mockMapper = new Mock<IMapper>();

            _postsService = new PostsService(_mockPostsRepository.Object, _mockFilesService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_NoDraftsNoTag_AllNonDraftPosts()
        {
            // Arrange
            var posts = GetPosts();
            var postsDto = GetPostsDto();

            _mockPostsRepository.Setup(x => x.GetAllAsync(false, null))
                .ReturnsAsync(posts);

            _mockMapper.Setup(x => x.Map<IEnumerable<PostDTO>>(It.IsAny<IEnumerable<Post>>()))
                .Returns(postsDto);

            // Act
            var result = await _postsService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), postsDto.Count());
            Assert.Equal(result.First().Id, postsDto.First().Id);
        }

        [Fact]
        public async Task GetBySlugAsync_SlugExists_PostFound()
        {
            // Arrange
            string slug = "this-is-a-slug";

            _mockPostsRepository.Setup(x => x.GetBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync(new Post { Id = 1, Slug = slug });

            _mockMapper.Setup(x => x.Map<PostDTO>(It.IsAny<Post>()))
                .Returns(new PostDTO { Id = 1, Slug = slug });

            // Act
            var result = await _postsService.GetBySlugAsync(slug);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(slug, result.Slug);
        }

        [Fact]
        public async Task GetBySlugAsync_SlugDoesntExist_ThrowsNotFoundException()
        {
            // Arrange
            string slug = "this-is-a-slug";

            _mockPostsRepository.Setup(x => x.GetBySlugAsync(It.IsAny<string>()))
                .ReturnsAsync(GetNullPost());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundAPIException>(async () => await _postsService.GetBySlugAsync(slug));
        }

        [Fact]
        public async Task GetByIdAsync_IdExists_PostFound()
        {
            // Arrange
            int id = 1;

            _mockPostsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { Id = id });

            _mockMapper.Setup(x => x.Map<PostDTO>(It.IsAny<Post>()))
                .Returns(new PostDTO { Id = 1 });

            // Act
            var result = await _postsService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_IdDoesntExist_ThrowsNotFoundException()
        {
            // Arrange
            int id = 1;

            _mockPostsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetNullPost());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundAPIException>(async () => await _postsService.GetByIdAsync(id));
        }

        [Fact]
        public async Task CreateAsync_PostForCreation_CreatedPost()
        {
            // Arrange
            var postForCreation = new PostForCreationDTO
            {
                Title = "Title 1",
                Content = "Post content",
                IsDraft = false,
                Summary = "Post summary",
                Tags = new[] { "tag1", "tag2" },
                Thumbnail = null
            };

            _mockMapper.Setup(x => x.Map<Post>(It.IsAny<PostForCreationDTO>()))
                .Returns(new Post
                {
                    Id = 1,
                    Title = "Title 1",
                    Content = "Post content",
                    IsDraft = false,
                    Summary = "Post summary",
                    Tags = "tag1,tag2",
                    Thumbnail = null
                });

            _mockPostsRepository.Setup(x => x.SlugExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockFilesService.Setup(x => x.SavePostThumbnailAsync(It.IsAny<int>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(GetNullString());

            _mockPostsRepository.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            _mockPostsRepository.Setup(x => x.SaveChangesAsync())
               .Returns(Task.CompletedTask);

            _mockMapper.Setup(x => x.Map<PostDTO>(It.IsAny<Post>()))
               .Returns(new PostDTO
               {
                   Id = 1,
                   Title = "Title 1",
                   Content = "Post content",
                   IsDraft = false,
                   Summary = "Post summary",
                   Tags = new[] { "tag1", "tag2" },
                   Thumbnail = null
               });

            // Act
            var result = await _postsService.CreateAsync(postForCreation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postForCreation.Title, result.Title);
        }

        [Fact]
        public async Task UpdateAsync_PostFound_UpdatedPost()
        {
            // Arrange
            var postForUpdate = new PostForUpdateDTO
            {
                Id = 1,
                Title = "Title 1",
                Content = "Post content",
                IsDraft = false,
                Summary = "Post summary",
                Tags = new[] { "tag1", "tag2" },
                Thumbnail = null
            };

            _mockPostsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { Id = 1, Title = "Title 1" });

            _mockMapper.Setup(x => x.Map(It.IsAny<PostForUpdateDTO>(), It.IsAny<Post>()))
               .Returns(new Post
               {
                   Id = 1,
                   Title = "Title 2",
                   Content = "Post content",
                   IsDraft = false,
                   Summary = "Post summary",
                   Tags = "tag1,tag2",
                   Thumbnail = null
               });

            _mockFilesService.Setup(x => x.SavePostThumbnailAsync(It.IsAny<int>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(GetNullString());

            _mockPostsRepository.Setup(x => x.SaveChangesAsync())
               .Returns(Task.CompletedTask);

            _mockMapper.Setup(x => x.Map<PostDTO>(It.IsAny<Post>()))
               .Returns(new PostDTO
               {
                   Id = 1,
                   Title = "Title 2",
                   Content = "Post content",
                   IsDraft = false,
                   Summary = "Post summary",
                   Tags = new[] { "tag1", "tag2" },
                   Thumbnail = null
               });

            // Act
            var result = await _postsService.UpdateAsync(postForUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Title 2", result.Title);
        }

        [Fact]
        public async Task UpdateAsync_PostNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var postForUpdate = new PostForUpdateDTO
            {
                Id = 1,
                Title = "Title 1",
                Content = "Post content",
                IsDraft = false,
                Summary = "Post summary",
                Tags = new[] { "tag1", "tag2" },
                Thumbnail = null
            };

            _mockPostsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetNullPost());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundAPIException>(async () => await _postsService.UpdateAsync(postForUpdate));
        }

        [Fact]
        public async Task DeleteAsync_PostExists_PostDeleted()
        {
            // Arrange
            int postId = 1;

            _mockPostsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Post { Id = postId })
               .Verifiable();

            _mockPostsRepository.Setup(x => x.Remove(It.IsAny<Post>()))
                .Verifiable();

            _mockPostsRepository.Setup(x => x.SaveChangesAsync())
             .Returns(Task.CompletedTask);

            _mockFilesService.Setup(x => x.DeletePostThumbnail(It.IsAny<int>())).Verifiable();

            // Act
            await _postsService.DeleteAsync(postId);

            // Assert
            Assert.True(true);
            _mockPostsRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
            _mockPostsRepository.Verify(x => x.Remove(It.IsAny<Post>()), Times.Once());
            _mockFilesService.Verify(x => x.DeletePostThumbnail(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_PostDoesntExists_ThrowsNotFoundException()
        {
            // Arrange
            int postId = 1;

            _mockPostsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(GetNullPost());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundAPIException>(async () => await _postsService.DeleteAsync(postId));
        }

        [Fact]
        public async Task GetAllTagsAsync_TagsExist_AllTags()
        {
            // Arrange
            _mockPostsRepository.Setup(x => x.GetAllTagsAsync())
                .ReturnsAsync(new[] { "tag1", "tag2" });

            // Act
            var result = await _postsService.GetAllTagsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        private static IEnumerable<Post> GetPosts()
        {
            return new List<Post>
            {
                new Post { Id = 1, Title = "" }
            };
        }

        private static IEnumerable<PostDTO> GetPostsDto()
        {
            return new List<PostDTO>
            {
                new PostDTO { Id = 1, Title = "" }
            };
        }

        private static Post? GetNullPost()
        {
            return null;
        }

        private static string? GetNullString()
        {
            return null;
        }
    }
}
