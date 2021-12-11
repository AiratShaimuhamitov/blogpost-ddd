using System.Threading;
using System.Threading.Tasks;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication.Models;

namespace Blogpost.Infrastructure.Authentication.Authenticators;

public class FacebookAuthenticator : IAuthenticator<FacebookAuthenticatorParameter>
{
    public bool IsExternalAuthenticator => true;

    public Task<AuthenticatorResult> Authenticate(FacebookAuthenticatorParameter parameter)
    {
        throw new System.NotImplementedException();
    }
}