using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class CreatePostEndpoint : EndpointBaseAsync.WithRequest<PostForCreationDTO>.WithActionResult<PostDTO>
    {
        private readonly IPostsService _postsService;

        public CreatePostEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpPost]
        [Route("api/posts")]
        [Authorize]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
           Summary = "Creates a post",
           Description = "Creates a new post.",
           OperationId = "Posts.Create",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult<PostDTO>> HandleAsync([FromForm] PostForCreationDTO postForCreation, CancellationToken cancellationToken = default)
        {
            var post = await _postsService.CreateAsync(postForCreation);

            return CreatedAtRoute("GetPost", new { post.Id }, post);
        }
    }
}

