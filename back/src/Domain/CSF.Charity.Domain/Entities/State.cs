using CSF.Charity.Domain.Core.Models;
using System;

namespace CSF.Charity.Domain.Entities
{
    public class State : Entity<Guid>
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
    }
}
