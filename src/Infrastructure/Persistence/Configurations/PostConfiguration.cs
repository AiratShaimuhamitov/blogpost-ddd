using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blogpost.Domain.Entities;

namespace Blogpost.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts").HasKey(k => k.Id);

        builder.Property(t => t.Content)
            .HasMaxLength(600)
            .IsRequired();

        builder.HasMany(x => x.Comments)
            .WithOne()
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(x => x.Likes)
            .WithOne()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}