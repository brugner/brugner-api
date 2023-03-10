using AutoMapper;
using Brugner.API.Core.Contracts.Repositories;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Exceptions;
using Brugner.API.Core.Extensions;
using Brugner.API.Core.Models.DTOs.Posts;
using Brugner.API.Core.Models.Entities;

namespace Brugner.API.Core.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;

        public PostsService(IPostsRepository postsRepository, IMapper mapper)
        {
            _postsRepository = postsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDTO>> GetAllAsync(bool includeDrafts = false, string? tag = null)
        {
            var posts = await _postsRepository.GetAllAsync(includeDrafts, tag);

            return _mapper.Map<IEnumerable<PostDTO>>(posts);
        }

        public async Task<PostDTO> GetBySlugAsync(string slug)
        {
            var post = await _postsRepository.GetBySlugAsync(slug);

            if (post == null)
            {
                throw new NotFoundAPIException($"Post with slug '{slug}' not found");
            }

            return _mapper.Map<PostDTO>(post);
        }

        public async Task<PostDTO> GetByIdAsync(int id)
        {
            var post = await _postsRepository.GetByIdAsync(id);

            if (post == null)
            {
                throw new NotFoundAPIException($"Post with Id '{id}' not found");
            }

            return _mapper.Map<PostDTO>(post);
        }

        public async Task<PostDTO> CreateAsync(PostForCreationDTO postForCreation)
        {
            var post = _mapper.Map<Post>(postForCreation);
            post.Slug = await SetSlugAsync(post.Title);
            post.CreatedAt = DateTime.Now;

            await _postsRepository.AddAsync(post);
            await _postsRepository.SaveChangesAsync();

            return _mapper.Map<PostDTO>(post);
        }

        public async Task<PostDTO> UpdateAsync(PostForUpdateDTO postForUpdate)
        {
            var post = await _postsRepository.GetByIdAsync(postForUpdate.Id);

            if (post == null)
            {
                throw new NotFoundAPIException($"Post with Id '{postForUpdate.Id}' not found");
            }

            _mapper.Map(postForUpdate, post);

            post.UpdatedAt = DateTime.Now;

            await _postsRepository.SaveChangesAsync();

            return _mapper.Map<PostDTO>(post);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _postsRepository.GetByIdAsync(id);

            if (post == null)
            {
                throw new NotFoundAPIException($"Post with Id '{id}' not found");
            }

            _postsRepository.Remove(post);
            await _postsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetAllTagsAsync()
        {
            return await _postsRepository.GetAllTagsAsync();
        }

        #region Helpers
        private async Task<string> SetSlugAsync(string title)
        {
            string slug = title.ToSlug();

            if (await _postsRepository.SlugExistsAsync(slug))
            {
                slug += "-" + DateTime.Now.Millisecond;
            }

            return slug;
        }
        #endregion
    }
}

