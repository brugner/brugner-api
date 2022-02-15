using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Contracts.Services;
using Brugner.API.Core.Extensions;
using Brugner.API.Core.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Brugner.API.Endpoints.Auth
{
    public class ChangePasswordEndpoint : EndpointBaseAsync.WithRequest<ChangePasswordDTO>.WithActionResult
    {
        private readonly IAuthService _authService;

        public ChangePasswordEndpoint(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("api/auth/change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
           Summary = "Change password",
           Description = "Allows a user to change its password.",
           OperationId = "Auth.ChangePassword",
           Tags = new[] { "Auth" })]
        public override async Task<ActionResult> HandleAsync(ChangePasswordDTO request, CancellationToken cancellationToken = default)
        {
            var userId = User.GetId();
            await _authService.ChangePasswordAsync(request, userId);

            return NoContent();
        }
    }
}

