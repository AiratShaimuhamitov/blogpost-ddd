using System.Threading;
using System.Threading.Tasks;
using Blogpost.Application.Common.Models;

namespace Blogpost.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResult> Login<TAuthenticatorParameter>(TAuthenticatorParameter parameter,
        CancellationToken cancellationToken = default)
        where TAuthenticatorParameter : AuthenticatorParameter;

    Task<AuthenticationResult> RefreshToken(string refreshToken, string ipAddress,
        CancellationToken cancellationToken = default);
}