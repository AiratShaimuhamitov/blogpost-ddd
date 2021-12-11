using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Posts.Commands.Unlike;

public class UnlikePostCommand : IRequest
{
    public Guid PostId { get; init; }

    public Guid ProfileId { get; init; }

    public class UnlikePostCommandHandler : AsyncRequestHandler<UnlikePostCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly PostsRepository _postsRepository;

        public UnlikePostCommandHandler(IApplicationDbContext context, PostsRepository postsRepository)
        {
            _context = context;
            _postsRepository = postsRepository;
        }

        protected override async Task Handle(UnlikePostCommand request, CancellationToken cancellationToken)
        {
            Post post = await _postsRepository.GetPostById(request.PostId, cancellationToken);

            Profile profile = await _context.Profiles
                .FindAsync(new object[] { request.ProfileId }, cancellationToken);
            if (profile == null) throw new NotFoundException("Profile", request.ProfileId);

            post.UnlikeFrom(profile);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}