using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication.Models;

namespace Blogpost.Infrastructure.Authentication
{
    public sealed class JwtFactory
    {
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtIssuerOptions = jwtOptions.Value;
        }

        public Token Generate(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Issuer = _jwtIssuerOptions.Issuer,
                IssuedAt = _jwtIssuerOptions.IssuedAt,
                Audience = _jwtIssuerOptions.Audience,
                Expires = _jwtIssuerOptions.Expiration,
                SigningCredentials = _jwtIssuerOptions.SigningCredentials
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new Token { Value = tokenHandler.WriteToken(securityToken), ExpiresAt = _jwtIssuerOptions.Expiration };
        }
    }
}