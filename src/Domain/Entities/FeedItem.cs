using System;
using System.Collections.Generic;
using System.Linq;
using Blogpost.Domain.Common;

namespace Blogpost.Domain.Entities;

public abstract class FeedItem<TKey> : AuditableEntity<TKey>
    where TKey : struct
{
    private readonly List<Like> _likes = new();
    public virtual IReadOnlyList<Like> Likes => _likes.ToList();

    public void PutLikeFrom(Profile profile)
    {
        if (_likes.Any(x => x.Profile == profile))
            throw new InvalidOperationException("There is a like from the user already");

        _likes.Add(new Like(profile));
    }

    public void UnlikeFrom(Profile profile)
    {
        Like like = _likes.Single(x => x.Profile == profile);
        _likes.Remove(like);
    }
}