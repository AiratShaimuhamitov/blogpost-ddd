using System.Threading.Tasks;
using Blogpost.Domain.Common;

namespace Blogpost.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}