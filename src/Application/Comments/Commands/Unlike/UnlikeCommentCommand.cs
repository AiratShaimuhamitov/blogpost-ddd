using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Comments.Commands.Unlike
{
    public class UnlikeCommentCommand : IRequest
    {
        public Guid CommentId { get; init; }

        public Guid ProfileId { get; init; }

        public class UnlikePostCommandHandler : AsyncRequestHandler<UnlikeCommentCommand>
        {
            private readonly IApplicationDbContext _applicationDbContext;

            private readonly CommentsRepository _commentsRepository;

            public UnlikePostCommandHandler(IApplicationDbContext applicationDbContext,
                CommentsRepository commentsRepository)
            {
                _applicationDbContext = applicationDbContext;
                _commentsRepository = commentsRepository;
            }

            protected override async Task Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentsRepository.GetById(request.CommentId, cancellationToken);

                var profile = await _applicationDbContext.Profiles
                    .FindAsync(new object[] { request.ProfileId }, cancellationToken);
                if (profile == null) throw new NotFoundException("Profile", request.ProfileId);

                comment.UnlikeFrom(profile);

                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}