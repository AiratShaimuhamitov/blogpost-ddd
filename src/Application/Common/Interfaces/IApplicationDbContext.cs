using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Blogpost.Domain.Entities;

namespace Blogpost.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Profile> Profiles { get; set; }

    DbSet<Post> Posts { get; set; }

    DbSet<Comment> Comments { get; set; }
    DbSet<Logbook> Logbooks { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;
}