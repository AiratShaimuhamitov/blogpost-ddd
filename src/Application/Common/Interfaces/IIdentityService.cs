using System;
using System.Threading.Tasks;
using Blogpost.Application.Common.Models;

namespace Blogpost.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<(Result Result, Guid UserId)> CreateUser(Guid id, string email, string password = null);

    public Task DeleteUser(Guid userId);
}