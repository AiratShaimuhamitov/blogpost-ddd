using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Repositories;

namespace Blogpost.Application.Posts.Commands.Like
{
    public class LikePostCommand : IRequest
    {
        public Guid PostId { get; init; }

        public Guid ProfileId { get; init; }

        public class LikePostCommandHandler : AsyncRequestHandler<LikePostCommand>
        {
            private readonly IApplicationDbContext _context;
            private readonly PostsRepository _postsRepository;

            public LikePostCommandHandler(IApplicationDbContext applicationDbContext,
                PostsRepository postsRepository)
            {
                _context = applicationDbContext;
                _postsRepository = postsRepository;
            }

            protected override async Task Handle(LikePostCommand request, CancellationToken cancellationToken)
            {
                var post = await _postsRepository.GetPostById(request.PostId, cancellationToken);

                var profile = await _context.Profiles
                    .FindAsync(new object[] { request.ProfileId }, cancellationToken);
                if (profile == null) throw new NotFoundException("Profile", request.ProfileId);

                post.PutLikeFrom(profile);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}