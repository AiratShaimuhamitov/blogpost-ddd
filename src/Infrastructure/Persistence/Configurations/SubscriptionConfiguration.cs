using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blogpost.Domain.Entities;

namespace Blogpost.Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Profile)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.Subscriber)
                .WithMany(x => x.Subscriptions)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}