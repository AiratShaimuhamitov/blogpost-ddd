using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;

namespace Blogpost.Application.Comments.Commands.Like
{
    public class LikeCommentCommand : IRequest
    {
        public Guid CommentId { get; init; }

        public Guid ProfileId { get; init; }

        public class LikeCommentCommandHandler : AsyncRequestHandler<LikeCommentCommand>
        {
            private readonly IApplicationDbContext _context;
            private readonly CommentsRepository _commentsRepository;

            public LikeCommentCommandHandler(IApplicationDbContext context, CommentsRepository commentsRepository)
            {
                _context = context;
                _commentsRepository = commentsRepository;
            }

            protected override async Task Handle(LikeCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentsRepository.GetById(request.CommentId, cancellationToken);

                var profile = await _context.Profiles
                    .FindAsync(new object[] { request.ProfileId }, cancellationToken);
                if (profile == null) throw new NotFoundException("Profile", request.ProfileId);

                comment.PutLikeFrom(profile);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}