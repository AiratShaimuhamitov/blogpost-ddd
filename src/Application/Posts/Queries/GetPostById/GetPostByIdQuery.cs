using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Posts.Queries.GetPostsWithPagination;

namespace Blogpost.Application.Posts.Queries.GetPostById;

public class GetPostByIdQuery : IRequest<PostDto>
{
    public Guid PostId { get; init; }

    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;

        public GetPostByIdQueryHandler(ICurrentUserService currentUserService,
            IConfiguration configuration)
        {
            _currentUserService = currentUserService;
            _configuration = configuration;
        }

        public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            PostDto post;
            await using (var db = new SqlConnection(connectionString))
            {
                post = await db.QuerySingleOrDefaultAsync<PostDto>(
                    @"select post.Id, Content, IsVisible, Created as CreatedAt, CreatedById, pr.Name as CreatedByName,
                                       (select count(*) from Likes where PostId = post.Id) as Likes,
                                       (select count(*) from Comments where PostId = post.Id) as Comments,
                                       (select count(*) from Likes where PostId = post.Id and ProfileId = @ProfileId) as HasLikeFromCurrentUser
                                from posts post
                                join profiles pr on post.CreatedById = pr.Id
                                where post.Id = @PostId",
                    new { ProfileId = _currentUserService.UserId, request.PostId });
            }

            if (post == null) throw new NotFoundException("Post", request.PostId);

            return post;
        }
    }
}