using System;
using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Infrastructure
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
    }
}