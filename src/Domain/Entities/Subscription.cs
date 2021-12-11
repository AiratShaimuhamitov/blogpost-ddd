using Blogpost.Domain.Common;

namespace Blogpost.Domain.Entities;

public class Subscription : Entity<long>
{
    public virtual Profile Profile { get; }

    public virtual Profile Subscriber { get; }

    protected Subscription() { }

    public Subscription(Profile profile, Profile subscriber)
        : this()
    {
        Profile = profile;
        Subscriber = subscriber;
    }
}