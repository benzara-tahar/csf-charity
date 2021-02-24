using CSF.Charity.Domain.Core.Models;
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
