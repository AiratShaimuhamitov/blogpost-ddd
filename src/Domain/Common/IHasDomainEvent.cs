using System.Collections.Generic;

namespace Blogpost.Domain.Common;

public interface IHasDomainEvent
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
}