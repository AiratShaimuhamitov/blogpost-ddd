using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blogpost.Infrastructure.Identity.Models;

namespace Blogpost.Infrastructure.Identity.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Token);

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.IsActive);
    }
}