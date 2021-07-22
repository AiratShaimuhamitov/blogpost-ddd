using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blogpost.Domain.Entities;

namespace Blogpost.Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments").HasKey(k => k.Id);

            builder.Property(p => p.Id);

            builder.Property(t => t.Content)
                .HasMaxLength(400)
                .IsRequired();

            builder.HasMany(x => x.SubComments)
                .WithOne(x => x.Parent)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(x => x.Likes)
                .WithOne()
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}