using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Blogpost.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}