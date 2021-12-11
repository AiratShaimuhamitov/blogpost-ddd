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

namespace Blogpost.Application.Profiles.Queries.GetSubscriptions;

public class GetSubscriptionsQuery : IRequest<List<ProfileDto>>
{
    public Guid ProfileId { get; init; }

    public class GetSubscriptionsQueryHandler : IRequestHandler<GetSubscriptionsQuery, List<ProfileDto>>
    {
        private readonly IConfiguration _configuration;

        public GetSubscriptionsQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ProfileDto>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            await using var db = new SqlConnection(connectionString);
            var profiles = await db.QueryAsync<ProfileDto>(
                @"select P.Id, P.Name from Subscription S
                                join Profiles P on S.ProfileId = P.Id
                                where S.SubscriberId = @ProfileId",
                new { request.ProfileId });

            return profiles.ToList();
        }
    }
}
