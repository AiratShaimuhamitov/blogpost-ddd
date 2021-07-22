using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Blogpost.Application.Common.Interfaces;

namespace Blogpost.Application.Comments.Queries.GetComments
{
    public class GetCommentsQuery : IRequest<List<CommentDto>>
    {
        public Guid PostId { get; init; }

        public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, List<CommentDto>>
        {
            private readonly IConfiguration _configuration;
            private readonly ICurrentUserService _currentUserService;

            public GetCommentsQueryHandler(IConfiguration configuration, ICurrentUserService currentUserService)
            {
                _configuration = configuration;
                _currentUserService = currentUserService;
            }

            public async Task<List<CommentDto>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                var lookup = new Dictionary<Guid, CommentDto>();
                await using var db = new SqlConnection(connectionString);

                db.Query<CommentDto, CommentDto, CommentDto>(
                    @"select CP.Id, CP.Content, CP.CreatedById as CreatedById, prCP.Name as CreatedByName, CP.Created as CreatedAt,
                               (select count(*) from Likes where CommentId = CP.Id) as Likes,
                               (select count(*) from Likes where CommentId = CP.Id and ProfileId = @ProfileId) as HasLikeFromCurrentUser,
                               CH.Id, CH.Content, CH.CreatedById as CreatedById, prCH.Name as CreatedByName, CH.Created as CreatedAt,
                               (select count(*) from Likes where CommentId = CH.Id) as Likes,
                               (select count(*) from Likes where CommentId = CH.Id and ProfileId = @ProfileId) as HasLikeFromCurrentUser
                               from comments CP
                               left join profiles prCP on CP.CreatedById = prCP.Id
                               left join comments CH on CP.Id = CH.ParentId
                               left join profiles prCH on CP.CreatedById = prCH.Id
                               where cp.PostId = @PostId",
                    (cp, ch) =>
                    {
                        if (!lookup.TryGetValue(cp.Id, out var comment))
                            lookup.Add(cp.Id, cp);
                        comment ??= cp;
                        if (ch != null) comment.SubComments.Add(ch);
                        return comment;
                    }, new { ProfileId = _currentUserService.UserId, request.PostId });

                return lookup.Values.ToList();
            }
        }
    }
}