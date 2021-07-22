using System;
using System.Threading;
using System.Threading.Tasks;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Repositories
{
    public class CommentsRepository
    {
        private readonly IApplicationDbContext _context;

        public CommentsRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments
                .FindAsync(new object[] { id }, cancellationToken);

            if (comment == null) throw new NotFoundException(nameof(Comment), id);

            await _context.Entry(comment).Collection(x => x.Likes).LoadAsync(cancellationToken);
            await _context.Entry(comment).Collection(x => x.SubComments).LoadAsync(cancellationToken);

            return comment;
        }
    }
}