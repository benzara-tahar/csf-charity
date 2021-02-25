using CSF.Charity.Domain.Core.Models;
using CSF.Charity.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace CSF.Charity.Domain.Entities
{
    public class TodoList : Entity<Guid>
    {

        public string Title { get; set; }

        public Colour Colour { get; set; } = Colour.White;

        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
