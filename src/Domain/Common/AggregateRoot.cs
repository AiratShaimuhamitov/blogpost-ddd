using System.Collections.Generic;

namespace Blogpost.Domain.Common;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IHasDomainEvent
    where TKey : struct
{
    private readonly List<DomainEvent> _domainEvents = new();
    public virtual IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected virtual void AddDomainEvent(DomainEvent newEvent)
    {
        _domainEvents.Add(newEvent);
    }

    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }

    protected AggregateRoot() { }

    public AggregateRoot(TKey id)
        : base(id)
    {
    }
}