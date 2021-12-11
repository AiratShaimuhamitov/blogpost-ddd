using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Profiles.Queries.Models;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Profiles.Queries.GetMyProfile;

public class GetMyProfileQuery : IRequest<MyProfileDto>
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MyProfileDto>
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;

        public GetMyProfileQueryHandler(IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        public async Task<MyProfileDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            var profileId = _currentUserService.UserId;

            await using var db = new SqlConnection(connectionString);
            var profile = await db.QuerySingleOrDefaultAsync<MyProfileDto>(
                @"select Id, Name, Emai from Profiles
                                where Id = @ProfileId",
                new { ProfileId = profileId!.Value });

            if (profile is null) throw new NotFoundException(nameof(Profile), profileId);

            return profile;
        }
    }
}
