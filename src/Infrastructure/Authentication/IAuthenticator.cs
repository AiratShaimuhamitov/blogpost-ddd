using System.Threading.Tasks;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication.Models;

namespace Blogpost.Infrastructure.Authentication;

public interface IAuthenticator<in TParameter>
    where TParameter : AuthenticatorParameter
{
    bool IsExternalAuthenticator { get; }

    Task<AuthenticatorResult> Authenticate(TParameter parameter);
}