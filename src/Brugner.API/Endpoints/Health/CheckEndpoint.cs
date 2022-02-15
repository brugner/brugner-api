using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Health
{
    public class CheckEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IPostsService _postsService;

        public CheckEndpoint(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        [Route("api/health/check")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
           Summary = "Tests if the API is alive",
           Description = "Tests if the API is working correctly by making a query to the database and returning some stats as a result.",
           OperationId = "Health.Check",
           Tags = new[] { "Health" })]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            var posts = await _postsService.GetAllAsync(includeDrafts: true);

            return Ok(new { Posts = posts.Count(), Drafts = posts.Count(x => x.IsDraft) });
        }
    }
}

