using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Identity.Models;

namespace Blogpost.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(Result Result, Guid UserId)> CreateUser(Guid id, string email,
            string password = null)
        {
            var user = new ApplicationUser
            {
                Id = id,
                UserName = email,
                Email = email
            };

            var result = string.IsNullOrEmpty(password)
                ? await _userManager.CreateAsync(user)
                : await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task DeleteUser(Guid userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.DeleteAsync(applicationUser);
        }
    }
}