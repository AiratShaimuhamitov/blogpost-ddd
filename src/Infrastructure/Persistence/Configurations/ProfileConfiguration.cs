using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blogpost.Domain.Entities;

namespace Blogpost.Infrastructure.Persistence.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Posts)
            .WithOne(x => x.CreatedBy);

        builder.Ignore(e => e.DomainEvents);
    }
}