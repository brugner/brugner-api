using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class PostsEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult<IEnumerable<PostDTO>>
    {
        private readonly IPostsService _postsService;

        public PostsEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        [Route("api/posts")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
           Summary = "Get all posts",
           Description = "Get all posts.",
           OperationId = "Posts.GetPosts",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult<IEnumerable<PostDTO>>> HandleAsync([FromQuery] string? tag, CancellationToken cancellationToken = default)
        {
            bool includeDrafts = User.Identity != null && User.Identity.IsAuthenticated;
            var posts = await _postsService.GetAllAsync(includeDrafts, tag);

            return Ok(posts);
        }
    }
}

