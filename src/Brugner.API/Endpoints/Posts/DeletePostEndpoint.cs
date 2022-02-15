using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class DeletePostEndpoint : EndpointBaseAsync.WithRequest<int>.WithActionResult
    {
        private readonly IPostsService _postsService;

        public DeletePostEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpDelete]
        [Route("api/posts/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
           Summary = "Deletes a post",
           Description = "Deletes the specified post.",
           OperationId = "Posts.Delete",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            await _postsService.DeleteAsync(id);

            return NoContent();
        }
    }
}

