using System;
using Microsoft.Extensions.DependencyInjection;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication.Authenticators;

namespace Blogpost.Infrastructure.Authentication
{
    public class AuthenticatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthenticatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAuthenticator<TParameter> Create<TParameter>()
            where TParameter : AuthenticatorParameter
        {
            if (typeof(TParameter) == typeof(EmailAuthenticatorParameter))
            {
                return (IAuthenticator<TParameter>)_serviceProvider.GetService<IAuthenticator<EmailAuthenticatorParameter>>();
            }
            if (typeof(TParameter) == typeof(GoogleAuthenticatorParameter))
            {
                return (IAuthenticator<TParameter>)_serviceProvider.GetService<IAuthenticator<GoogleAuthenticatorParameter>>();
            }
            if (typeof(TParameter) == typeof(FacebookAuthenticatorParameter))
            {
                return (IAuthenticator<TParameter>)_serviceProvider.GetService<IAuthenticator<FacebookAuthenticatorParameter>>();
            }

            throw new InvalidOperationException(
                $"There is no authenticator that can handle {typeof(TParameter)} parameter");
        }
    }
}