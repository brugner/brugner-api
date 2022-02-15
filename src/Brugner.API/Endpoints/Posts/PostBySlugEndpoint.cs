using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class PostBySlugEndpoint : EndpointBaseAsync.WithRequest<string>.WithActionResult<PostDTO>
    {
        private readonly IPostsService _postsService;

        public PostBySlugEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        [Route("api/posts/slug/{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
           Summary = "Get the post with the specified tag",
           Description = "Get the post with the specified tag.",
           OperationId = "Posts.GetBySlug",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult<PostDTO>> HandleAsync([FromRoute] string slug, CancellationToken cancellationToken = default)
        {
            var post = await _postsService.GetBySlugAsync(slug);

            return Ok(post);
        }
    }
}

