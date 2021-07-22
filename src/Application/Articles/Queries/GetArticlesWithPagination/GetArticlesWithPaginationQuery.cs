using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Models;

namespace Blogpost.Application.Articles.Queries.GetArticlesWithPagination
{
    public class GetArticlesWithPaginationQuery : IRequest<PaginatedList<ArticleDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public class GetArticlesWithPaginationQueryHandler : IRequestHandler<GetArticlesWithPaginationQuery,
                PaginatedList<ArticleDto>>
        {
            private readonly IConfiguration _configuration;

            public GetArticlesWithPaginationQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<PaginatedList<ArticleDto>> Handle(GetArticlesWithPaginationQuery request, CancellationToken cancellationToken)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                IEnumerable<ArticleDto> articlesDto;
                int total;
                await using (var db = new SqlConnection(connectionString))
                {
                    total = await db.QuerySingleAsync<int>(@"select count(*) from articles");

                    articlesDto = await db.QueryAsync<ArticleDto>(
                        @"select title, content from articles
                                order by Created desc
                                offset @Skip rows
                                fetch next @Take rows only;",
                        new { Skip = (request.PageNumber - 1) * request.PageSize, Take = request.PageSize });
                }

                return new PaginatedList<ArticleDto>(articlesDto.ToList(), total, request.PageNumber, request.PageSize);
            }
        }
    }
}