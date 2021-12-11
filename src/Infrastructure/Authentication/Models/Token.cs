using System;

namespace Blogpost.Infrastructure.Authentication.Models;

public class Token
{
    public string Value { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}