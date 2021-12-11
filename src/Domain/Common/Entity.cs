using System;
using System.Collections.Generic;

namespace Blogpost.Domain.Common;

public abstract class Entity<TKey>
    where TKey : struct
{
    public TKey Id { get; }

    protected Entity()
    {
    }

    protected Entity(TKey id)
        : this()
    {
        Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<TKey> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        if (EqualityComparer<TKey>.Default.Equals(Id, default) ||
            EqualityComparer<TKey>.Default.Equals(other.Id, default))
            return false;

        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    private Type GetRealType()
    {
        Type type = GetType();

        return type.ToString().Contains("Castle.Proxies.") ? type.BaseType : type;
    }

    public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id.GetHashCode()).GetHashCode();
    }
}