using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class UpdatePostEndpoint : EndpointBaseAsync.WithRequest<PostForUpdateDTO>.WithActionResult<PostDTO>
    {
        private readonly IPostsService _postsService;

        public UpdatePostEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpPut]
        [Route("api/posts/{id}")]
        [Authorize]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
           Summary = "Updates a post",
           Description = "Updates the specified post.",
           OperationId = "Posts.Update",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult<PostDTO>> HandleAsync([FromForm] PostForUpdateDTO postForUpdate, CancellationToken cancellationToken = default)
        {
            var post = await _postsService.UpdateAsync(postForUpdate);

            return Ok(post);
        }
    }
}

