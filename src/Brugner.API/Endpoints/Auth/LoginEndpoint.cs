using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Auth
{
    public class LoginEndpoint : EndpointBaseAsync.WithRequest<UserForAuthDTO>.WithActionResult<AuthResultDTO>
    {
        private readonly IAuthService _authService;

        public LoginEndpoint(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("api/auth/login")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
           Summary = "Authenticates a user",
           Description = "Provided the correct email and password, a JWT token will be generated.",
           OperationId = "Auth.Login",
           Tags = new[] { "Auth" })]
        public override async Task<ActionResult<AuthResultDTO>> HandleAsync([FromBody] UserForAuthDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(request);

            return Ok(result);
        }
    }
}

