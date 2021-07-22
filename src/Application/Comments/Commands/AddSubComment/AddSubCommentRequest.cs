using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Comments.Commands.AddSubComment
{
    public class AddSubCommentRequest : IRequest<Guid>
    {
        public Guid CommentId { get; init; }

        public string Content { get; init; }

        public class AddSubCommentRequestHandler : IRequestHandler<AddSubCommentRequest, Guid>
        {
            private readonly IApplicationDbContext _applicationDbContext;

            public AddSubCommentRequestHandler(IApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public async Task<Guid> Handle(AddSubCommentRequest request, CancellationToken cancellationToken)
            {
                var comment = await _applicationDbContext.Comments
                    .FindAsync(new object[] { request.CommentId }, cancellationToken);
                if (comment == null) throw new NotFoundException(nameof(Comment), request.CommentId);

                var subComment = comment.AddSubComment(request.Content);

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return subComment.Id;
            }
        }
    }
}