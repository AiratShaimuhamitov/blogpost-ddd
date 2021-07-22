using System;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.Infrastructure.Authentication;
using Blogpost.Infrastructure.Authentication.Authenticators;
using Blogpost.Infrastructure.Identity;
using Blogpost.Infrastructure.Identity.Models;
using Blogpost.Infrastructure.Persistence;
using Blogpost.Infrastructure.Services;
using AuthenticationService = Blogpost.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = Blogpost.Application.Common.Interfaces.IAuthenticationService;

namespace Blogpost.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("BlogpostDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                        .UseLazyLoadingProxies());
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IDateTime, DateTimeService>();

            AddAuthentication(services, configuration);

            AddIdentity(services);
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IAuthenticator<EmailAuthenticatorParameter>, EmailAuthenticator>();
            services.AddTransient<IAuthenticator<GoogleAuthenticatorParameter>, GoogleAuthenticator>();
            services.AddTransient<IAuthenticator<FacebookAuthenticatorParameter>, FacebookAuthenticator>();
            services.AddTransient<AuthenticatorFactory>();
            services.AddTransient<JwtFactory>();

            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:Secret"]));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = configuration["JwtIssuerOptions:Issuer"];
                options.Audience = configuration["JwtIssuerOptions:Audience"];
                options.SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddIdentityServerJwt()
                .AddJwtBearer(options =>
                {
                    options.ClaimsIssuer = configuration["JwtIssuerOptions:Issuer"];
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtIssuerOptions:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["JwtIssuerOptions:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = secretKey,

                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();
        }
    }
}