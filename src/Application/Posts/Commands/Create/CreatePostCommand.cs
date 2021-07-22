using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Posts.Commands.Create
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public string Content { get; init; }

        public bool IsVisible { get; init; }

        public Guid CreatorId { get; init; }

        public class CreatePostCommandHandled : IRequestHandler<CreatePostCommand, Guid>
        {
            private readonly IApplicationDbContext _applicationDbContext;
            private readonly ILogger<CreatePostCommandHandled> _logger;

            public CreatePostCommandHandled(IApplicationDbContext applicationDbContext,
                ILogger<CreatePostCommandHandled> logger)
            {
                _applicationDbContext = applicationDbContext;
                _logger = logger;
            }

            public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                var post = new Post(request.Content, request.IsVisible);

                await _applicationDbContext.Posts.AddAsync(post, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("A new post with id = {PostId} was created by {CreatorId}",
                    post.Id,
                    request.CreatorId);

                return post.Id;
            }
        }
    }
}