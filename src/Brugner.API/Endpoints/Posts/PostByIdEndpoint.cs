using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class PostByIdEndpoint : EndpointBaseAsync.WithRequest<int>.WithActionResult<PostDTO>
    {
        private readonly IPostsService _postsService;

        public PostByIdEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        [Route("api/posts/{id}", Name = "GetPost")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
           Summary = "Get post",
           Description = "Get the post with the specified Id.",
           OperationId = "Posts.GetById",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult<PostDTO>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var post = await _postsService.GetByIdAsync(id);

            return Ok(post);
        }
    }
}

