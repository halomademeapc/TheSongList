using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TheSongList.Authorization
{
    public class HasEmailRequirement : IAuthorizationRequirement
    {
        public List<string> permittedEmails { get; private set; }

        public HasEmailRequirement(List<string> permittedEmails) => this.permittedEmails = permittedEmails;
    }
}
