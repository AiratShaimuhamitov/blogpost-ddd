using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Comments.Commands.AddComment;

public class AddCommentRequest : IRequest<Guid>
{
    public Guid PostId { get; init; }

    public string Content { get; init; }

    public class AddCommentRequestHandler : IRequestHandler<AddCommentRequest, Guid>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AddCommentRequestHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Guid> Handle(AddCommentRequest request, CancellationToken cancellationToken)
        {
            Post post = await _applicationDbContext.Posts
                .FindAsync(new object[] { request.PostId }, cancellationToken);

            if (post == null) throw new NotFoundException("Post", request.PostId);

            Comment comment = post.AddComment(request.Content);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return comment.Id;
        }
    }
}