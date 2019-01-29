using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TheSongList.Authorization
{
    public class HasEmailHandler : AuthorizationHandler<HasEmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasEmailRequirement requirement)
        {
            var emails = requirement.permittedEmails.Select(e => e.ToLower());

            if (emails == null || !emails.Any() || !context.User.HasClaim(c => c.Type == ClaimTypes.Email))
                return Task.CompletedTask;

            var email = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;

            if (emails.Contains(email.ToLower()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
