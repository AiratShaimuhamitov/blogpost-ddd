using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.Application.Posts.Queries.GetPostsWithPagination;

namespace Blogpost.Application.Posts.Queries.GetMyPostsWithPagination
{
    public class GetMyPostsWithPaginationQuery : IRequest<PaginatedList<PostDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public bool IsVisible { get; init; } = true;

        public class GetMyPostsWithPaginationQueryHandler : IRequestHandler<GetMyPostsWithPaginationQuery, PaginatedList<PostDto>>
        {
            private readonly IConfiguration _configuration;
            private readonly ICurrentUserService _currentUserService;

            public GetMyPostsWithPaginationQueryHandler(ICurrentUserService currentUserService,
                IConfiguration configuration)
            {
                _currentUserService = currentUserService;
                _configuration = configuration;
            }

            public async Task<PaginatedList<PostDto>> Handle(GetMyPostsWithPaginationQuery request, CancellationToken cancellationToken)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                IEnumerable<PostDto> postsDto;
                int total;
                await using (var db = new SqlConnection(connectionString))
                {
                    total = await db.QuerySingleAsync<int>(@"select count(*) from posts where IsVisible = @IsVisible",
                        new { IsVisible = request.IsVisible });

                    postsDto = await db.QueryAsync<PostDto>(
                        @"select post.Id, Content, IsVisible, Created as CreatedAt, CreatedById as CreatedBy, pr.Name as CreatedByName,
                               (select count(*) from Likes where PostId = post.Id) as Likes,
                               (select count(*) from Comments where PostId = post.Id) as Comments,
                               (select count(*) from Likes where PostId = post.Id and ProfileId = @ProfileId) as HasLikeFromCurrentUser
                                from posts post
                                join profiles pr on post.CreatedById = pr.Id
                                where post.IsVisible = @IsVisible
                                order by Created desc
                                offset @Skip rows
                                fetch next @Take rows only;",
                        new
                        {
                            ProfileId = _currentUserService.UserId,
                            IsVisible = request.IsVisible,
                            Skip = (request.PageNumber - 1) * request.PageSize,
                            Take = request.PageSize
                        });
                }

                return new PaginatedList<PostDto>(postsDto.ToList(), total, request.PageNumber, request.PageSize);
            }
        }
    }
}