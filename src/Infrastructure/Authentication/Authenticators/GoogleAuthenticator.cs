using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication.Models;

namespace Blogpost.Infrastructure.Authentication.Authenticators;

public class GoogleAuthenticator : IAuthenticator<GoogleAuthenticatorParameter>
{
    private readonly ILogger<GoogleAuthenticator> _logger;

    public GoogleAuthenticator(ILogger<GoogleAuthenticator> logger)
    {
        _logger = logger;
    }

    public bool IsExternalAuthenticator => true;

    public async Task<AuthenticatorResult> Authenticate(GoogleAuthenticatorParameter parameter)
    {
        try
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature
                .ValidateAsync(parameter.Token, new GoogleJsonWebSignature.ValidationSettings());

            return AuthenticatorResult.Success(payload.Name, payload.Email);
        }
        catch (InvalidJwtException e)
        {
            _logger.LogError(e, "Google jwt token is invalid");
            return AuthenticatorResult.Failure("Google jwt token is invalid");
        }
    }
}