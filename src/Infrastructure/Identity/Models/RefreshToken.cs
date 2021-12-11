using System;

namespace Blogpost.Infrastructure.Identity.Models;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string CreatedByIp { get; set; }

    public virtual ApplicationUser User { get; set; }

    public bool IsActive => DateTime.UtcNow <= ExpiresAt;
}