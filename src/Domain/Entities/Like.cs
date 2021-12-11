using Blogpost.Domain.Common;

namespace Blogpost.Domain.Entities;

public class Like : Entity<long>
{
    public virtual Profile Profile { get; private set; }

    protected Like() { }

    internal Like(Profile profile)
        : this()
    {
        Profile = profile;
    }
}