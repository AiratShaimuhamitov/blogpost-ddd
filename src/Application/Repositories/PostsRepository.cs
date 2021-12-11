using System;
using System.Threading;
using System.Threading.Tasks;
using Blogpost.Application.Common.Exceptions;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Repositories;

public class PostsRepository
{
    private readonly IApplicationDbContext _context;

    public PostsRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Post> GetPostById(Guid id, CancellationToken cancellationToken = default)
    {
        Post post = await _context.Posts
            .FindAsync(new object[] { id }, cancellationToken);

        if (post == null) throw new NotFoundException("Post", id);

        await _context.Entry(post).Collection(x => x.Likes).LoadAsync(cancellationToken);

        return post;
    }
}