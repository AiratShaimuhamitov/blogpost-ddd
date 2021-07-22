using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Posts.Commands.Delete
{
    public class DeletePostCommand : IRequest
    {
        public Guid PostId { get; init; }

        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
        {
            private readonly IApplicationDbContext _applicationDbContext;
            private readonly ILogger<DeletePostCommandHandler> _logger;

            public DeletePostCommandHandler(IApplicationDbContext applicationDbContext,
                ILogger<DeletePostCommandHandler> logger)
            {
                _applicationDbContext = applicationDbContext;
                _logger = logger;
            }

            public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                // including all child properties is required for client-cascade delete
                var post = await _applicationDbContext.Posts
                    .Include(x => x.Comments)
                        .ThenInclude(x => x.Likes)
                    .Include(x => x.Comments)
                        .ThenInclude(x => x.SubComments)
                            .ThenInclude(x => x.Likes)
                    .Include(x => x.Likes)
                    .SingleOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);
                if (post == null) throw new NotFoundException(nameof(Post), request.PostId);

                _applicationDbContext.Posts.Remove(post);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Post {PostId} was deleted", post.Id);

                return Unit.Value;
            }
        }
    }
}