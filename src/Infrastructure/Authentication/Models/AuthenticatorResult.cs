using System.Collections.Generic;
using Blogpost.Application.Common.Models;

namespace Blogpost.Infrastructure.Authentication.Models;

public class AuthenticatorResult : Result
{
    public string Name { get; private init; }

    public string Email { get; private init; }

    private AuthenticatorResult(bool succeeded, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
    }

    public static AuthenticatorResult Success(string name, string email)
    {
        return new(true, System.Array.Empty<string>())
        {
            Name = name,
            Email = email
        };
    }

    public new static AuthenticatorResult Success()
    {
        return new(true, System.Array.Empty<string>());
    }

    public new static AuthenticatorResult Failure(string errors)
    {
        return new(false, new[] { errors });
    }
}