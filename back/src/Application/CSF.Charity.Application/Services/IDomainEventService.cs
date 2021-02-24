using CSF.Charity.Domain.Core.Models;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Services
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
