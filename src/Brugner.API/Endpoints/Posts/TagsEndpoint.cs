using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Posts
{
    public class TagsEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult<IEnumerable<string>>
    {
        private readonly IPostsService _postsService;

        public TagsEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        [Route("api/posts/tags")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
           Summary = "Get all tags",
           Description = "Get all tags.",
           OperationId = "Posts.GetTags",
           Tags = new[] { "Posts" })]
        public override async Task<ActionResult<IEnumerable<string>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var tags = await _postsService.GetAllTagsAsync();

            return Ok(tags);
        }
    }
}

