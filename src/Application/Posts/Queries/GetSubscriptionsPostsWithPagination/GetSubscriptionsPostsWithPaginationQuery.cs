using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Models;
using Blogpost.Application.Posts.Queries.GetPostsWithPagination;

namespace Blogpost.Application.Posts.Queries.GetSubscriptionsPostsWithPagination;

public class GetSubscriptionsPostsWithPaginationQuery : IRequest<PaginatedList<PostDto>>
{
    public Guid ProfileId { get; set; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public class GetSubscriptionsPostsWithPaginationQueryHandler : IRequestHandler<GetSubscriptionsPostsWithPaginationQuery,
        PaginatedList<PostDto>>
    {
        private readonly IConfiguration _configuration;

        public GetSubscriptionsPostsWithPaginationQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<PaginatedList<PostDto>> Handle(GetSubscriptionsPostsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            IEnumerable<PostDto> postsDto;
            int total;
            await using (var db = new SqlConnection(connectionString))
            {
                total = await db.QuerySingleAsync<int>(@"select count(*) from posts
                                                                    where CreatedById in (select ProfileId from Subscription
                                                                        where SubscriberId = @ProfileId)
                                                                        and IsVisible = 1",
                    new { ProfileId = request.ProfileId });

                postsDto = await db.QueryAsync<PostDto>(
                    @"select post.Id, Content, Created as CreatedAt, CreatedById as CreatedBy, pr.Name as CreatedByName,
                                       (select count(*) from Likes where PostId = post.Id) as Likes,
                                       (select count(*) from Comments where PostId = post.Id) as Comments,
                                       (select count(*) from Likes where PostId = post.Id and ProfileId = @ProfileId) as HasLikeFromCurrentUser
                                       from Posts post
                                join profiles pr on post.CreatedById = pr.Id
                                where CreatedById in (select ProfileId from Subscription
                                                      where SubscriberId = @ProfileId)
                                    and IsVisible = 1
                                order by Created desc
                                offset @Skip rows
                                    fetch next @Take rows only;",
                    new
                    {
                        ProfileId = request.ProfileId,
                        Skip = (request.PageNumber - 1) * request.PageSize,
                        Take = request.PageSize
                    });
            }

            return new PaginatedList<PostDto>(postsDto.ToList(), total, request.PageNumber, request.PageSize);
        }
    }
}