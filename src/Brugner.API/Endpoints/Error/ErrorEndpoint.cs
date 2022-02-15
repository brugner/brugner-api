using System;
using Ardalis.ApiEndpoints;
using Brugner.API.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Brugner.API.Endpoints.Error
{
    public class ErrorEndpoint : EndpointBaseSync.WithoutRequest.WithActionResult
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public override ActionResult Handle()
        {
            int code = StatusCodes.Status500InternalServerError;
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            if (context.Error is InvalidArgumentAPIException)
                code = StatusCodes.Status400BadRequest;

            if (context.Error is NotFoundAPIException)
                code = StatusCodes.Status404NotFound;

            return Problem(statusCode: code, title: context.Error.Message, instance: context.Path);
        }
    }
}

