using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blogpost.Domain.Entities;

namespace Blogpost.Infrastructure.Persistence.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(60);

            builder.Property(x => x.Content).HasMaxLength(150);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}