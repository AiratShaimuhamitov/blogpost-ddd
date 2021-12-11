using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Comments.Commands.Delete;

public class DeleteCommentCommand : IRequest
{
    public Guid CommentId { get; init; }

    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;

        public DeleteCommentCommandHandler(IApplicationDbContext applicationDbContext,
            ILogger<DeleteCommentCommandHandler> logger)
        {
            _context = applicationDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            Comment comment = _context.Comments
                .Include(x => x.Likes)
                .Include(x => x.SubComments)
                .ThenInclude(x => x.Likes)
                .SingleOrDefault(x => x.Id == request.CommentId);
            if (comment == null) throw new NotFoundException(nameof(Comment), request.CommentId);

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Comment {CommentId} was deleted",
                request.CommentId);

            return Unit.Value;
        }
    }
}
