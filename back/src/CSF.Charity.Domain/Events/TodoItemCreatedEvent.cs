using CSF.Charity.Domain.Common;
using CSF.Charity.Domain.Entities;

namespace CSF.Charity.Domain.Events
{
    public class TodoItemCreatedEvent : DomainEvent
    {
        public TodoItemCreatedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
