using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Profiles.Queries.Models;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Profiles.Queries.GetProfileById
{
    public class GetProfileByIdQuery : IRequest<ProfileDto>
    {
        public Guid ProfileId { get; set; }

        public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, ProfileDto>
        {
            private readonly IConfiguration _configuration;

            public GetProfileByIdQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<ProfileDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                await using (var db = new SqlConnection(connectionString))
                {
                    var profile = await db.QuerySingleOrDefaultAsync<ProfileDto>(
                        @"select Id, Name from Profiles
                                where Id = @ProfileId",
                        new { ProfileId = request.ProfileId });

                    if (profile is null) throw new NotFoundException(nameof(Profile), request.ProfileId);

                    return profile;
                }
            }
        }
    }
}