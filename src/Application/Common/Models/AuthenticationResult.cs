using System;
using System.Collections.Generic;

namespace Blogpost.Application.Common.Models;

public class AuthenticationResult : Result
{
    public string AccessToken { get; set; }

    public DateTime ExpiresAt { get; set; }

    public string RefreshToken { get; set; }

    private AuthenticationResult(bool succeeded, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
    }

    public static AuthenticationResult Success(string accessToken, DateTime expiresAt, string refreshToken)
    {
        return new AuthenticationResult(true, System.Array.Empty<string>())
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            RefreshToken = refreshToken
        };
    }

    public new static AuthenticationResult Failure(IEnumerable<string> errors)
    {
        return new AuthenticationResult(false, errors);
    }

    public new static AuthenticationResult Failure(string errors)
    {
        return new AuthenticationResult(false, new[] { errors });
    }
}