using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Blogpost.Application.LogBook.Queries.GetLogs
{
    public class GetLogsQuery : IRequest<IEnumerable<LogDto>>
    {
        public Guid ProfileId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public class GetLogsPerMonthQueryHandler : IRequestHandler<GetLogsQuery, IEnumerable<LogDto>>
        {
            private readonly IConfiguration _configuration;

            public GetLogsPerMonthQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<IEnumerable<LogDto>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                await using (var db = new SqlConnection(connectionString))
                {
                    var logDto = await db.QueryAsync<LogDto>(
                        @"select * from Logbooks
                            where ProfileId = @ProfileId
                                 and LogDate between @from and @to",
                        new { request.ProfileId, request.From, request.To });

                    return logDto;
                }
            }
        }
    }
}