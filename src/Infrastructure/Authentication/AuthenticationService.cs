using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.Application.Repositories;
using Blogpost.Domain.Entities;
using Blogpost.Infrastructure.Authentication.Models;
using Blogpost.Infrastructure.Identity.Models;
using Blogpost.Infrastructure.Persistence;
using RefreshToken = Blogpost.Infrastructure.Identity.Models.RefreshToken;

namespace Blogpost.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticatorFactory _authenticatorFactory;
    private readonly JwtFactory _jwtFactory;
    private readonly ProfilesRepository _profilesRepository;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(AuthenticatorFactory authenticatorFactory,
        JwtFactory jwtFactory,
        ProfilesRepository profilesRepository,
        ApplicationDbContext applicationDbContext,
        ILogger<AuthenticationService> logger)
    {
        _authenticatorFactory = authenticatorFactory;
        _jwtFactory = jwtFactory;
        _profilesRepository = profilesRepository;
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    public async Task<AuthenticationResult> Login<TAuthenticatorParameter>(TAuthenticatorParameter parameter,
        CancellationToken cancellationToken = default)
        where TAuthenticatorParameter : AuthenticatorParameter
    {
        var authenticator = _authenticatorFactory.Create<TAuthenticatorParameter>();
        AuthenticatorResult result = await authenticator.Authenticate(parameter);
        if (!result.Succeeded) return AuthenticationResult.Failure(result.Errors);

        ApplicationUser applicationUser = await _applicationDbContext.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Email == result.Email, cancellationToken);
        if (applicationUser is null && authenticator.IsExternalAuthenticator)
        {
            _logger.LogInformation("Application user {Email} was not found in database after authentication, authenticator - {Authenticator}", result.Email, authenticator.GetType().Name);
            // user does not exist in our system cause an external authentication method was used for the first time
            applicationUser = await CreateApplicationUser(result.Email, result.Name, cancellationToken);
        }
        if (applicationUser == null) return AuthenticationResult.Failure($"Application user with email {result.Email} was not found in database");

        Token accessToken = _jwtFactory.Generate(applicationUser.Id);
        RefreshToken refreshToken = GenerateRefreshToken(parameter.IpAddress);

        applicationUser.RefreshTokens.Add(refreshToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return AuthenticationResult.Success(accessToken.Value, accessToken.ExpiresAt, refreshToken.Token);
    }

    public async Task<AuthenticationResult> RefreshToken(string token, string ipAddress,
        CancellationToken cancellationToken = default)
    {
        RefreshToken refreshToken = await _applicationDbContext.RefreshTokens
            .Include(x => x.User)
            .ThenInclude(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (!ValidateRefreshToken(token, refreshToken, out AuthenticationResult failure)) return failure;

        ApplicationUser applicationUser = refreshToken.User;

        Token accessToken = _jwtFactory.Generate(applicationUser.Id);
        RefreshToken newRefreshToken = GenerateRefreshToken(ipAddress);

        RemoveAllRefreshTokenAndAddNew(applicationUser, newRefreshToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return AuthenticationResult.Success(accessToken.Value, accessToken.ExpiresAt, newRefreshToken.Token);
    }

    private bool ValidateRefreshToken(string token, RefreshToken refreshToken, out AuthenticationResult failure)
    {
        if (refreshToken is null)
        {
            _logger.LogWarning("Refresh token {RefreshToken} was not found in database", token);
            {
                failure = AuthenticationResult.Failure("The refresh token was not found");
                return false;
            }
        }

        if (!refreshToken.IsActive)
        {
            _logger.LogWarning("Refresh token {RefreshToken} was expired", token);
            {
                failure = AuthenticationResult.Failure("The refresh token was expired");
                return false;
            }
        }

        failure = null;
        return true;
    }

    private void RemoveAllRefreshTokenAndAddNew(ApplicationUser applicationUser, RefreshToken refreshToken)
    {
        _applicationDbContext.RefreshTokens.RemoveRange(applicationUser.RefreshTokens);
        applicationUser.RefreshTokens = new List<RefreshToken> { refreshToken };
    }

    private async Task<ApplicationUser> CreateApplicationUser(string email, string name, CancellationToken cancellationToken)
    {
        var profile = new Profile(Guid.NewGuid(), name, email);
        await _profilesRepository.Add(profile, cancellationToken: cancellationToken);

        return await _applicationDbContext.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Id == profile.Id, cancellationToken);
    }

    private static RefreshToken GenerateRefreshToken(string ipAddress)
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(64);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}
