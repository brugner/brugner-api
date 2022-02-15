using System;
using System.Security.Claims;
using Brugner.API.Core.Constants;
using Brugner.API.Core.Exceptions;

namespace Brugner.API.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            var claim = user.Claims.SingleOrDefault(x => x.Type == AppClaims.Id);

            if (claim is null)
            {
                throw new InvalidArgumentAPIException("Invalid user Id claim");
            }

            return int.Parse(claim.Value);
        }
    }
}
