using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Blogpost.Application.Common.Interfaces;

namespace Blogpost.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            if (Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name), out Guid userId))
            {
                return userId;
            }

            return null;
        }
    }
}