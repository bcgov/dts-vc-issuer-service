using System;
using System.Security.Claims;

namespace Issuer
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Returns the Guid of the logged in user. If there is no logged in user, this will return Guid.Empty
        /// </summary>
        public static Guid GetissuerUserId(this ClaimsPrincipal User)
        {
            string userId = User?.Identity?.Name;
            return userId == null ? Guid.Empty : new Guid(userId);
        }

    }
}
