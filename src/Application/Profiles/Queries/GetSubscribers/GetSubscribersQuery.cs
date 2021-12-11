using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Profiles.Queries.Models;

namespace Blogpost.Application.Profiles.Queries.GetSubscribers;

public class GetSubscribersQuery : IRequest<List<ProfileDto>>
{
    public Guid ProfileId { get; set; }

    public class GetSubscribersQueryHandler : IRequestHandler<GetSubscribersQuery, List<ProfileDto>>
    {
        private readonly IConfiguration _configuration;

        public GetSubscribersQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ProfileDto>> Handle(GetSubscribersQuery request, CancellationToken cancellationToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            await using var db = new SqlConnection(connectionString);
            var profiles = await db.QueryAsync<ProfileDto>(
                @"select SubscriberId as Id, P.Name from Subscription S
                                join Profiles P on S.SubscriberId = P.Id
                                where S.ProfileId = @ProfileId",
                new { ProfileId = request.ProfileId });

            return profiles.ToList();
        }
    }
}
