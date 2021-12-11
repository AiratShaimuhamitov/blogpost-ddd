using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication.Models;
using Blogpost.Infrastructure.Identity.Models;

namespace Blogpost.Infrastructure.Authentication.Authenticators;

// ReSharper disable once ClassNeverInstantiated.Global
public class EmailAuthenticator : IAuthenticator<EmailAuthenticatorParameter>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EmailAuthenticator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public bool IsExternalAuthenticator => false;

    public async Task<AuthenticatorResult> Authenticate(EmailAuthenticatorParameter parameter)
    {
        ApplicationUser userProfile = await _userManager.FindByNameAsync(parameter.Email);
        if (userProfile == null)
            throw new NotFoundException($"Profile with email - {parameter.Email} was not found");

        if (!await _userManager.HasPasswordAsync(userProfile))
            return AuthenticatorResult.Failure("User does not have a password.");

        if (!await _userManager.CheckPasswordAsync(userProfile, parameter.Password))
            return AuthenticatorResult.Failure("Invalid password for user.");

        return AuthenticatorResult.Success(userProfile.UserName, userProfile.Email);
    }
}