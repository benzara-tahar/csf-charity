using CSF.Charity.Domain.Common;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
