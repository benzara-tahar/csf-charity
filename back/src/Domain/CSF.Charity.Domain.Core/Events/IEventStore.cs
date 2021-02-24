using NetDevPack.Messaging;

namespace CSF.Charity.Domain.Core.Events
{
    public interface IEventStore
    {
        void Save<T>(T theEvent) where T : Event;
    }
}