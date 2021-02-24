using CSF.Charity.Domain.Core.Models;
using System;

namespace CSF.Charity.Domain.Entities
{
    public class Township : Entity<Guid>
    {
        public Guid StateId { get; set; }
        public string Name{ get; set; }
        public string NameLatin { get; set; }
    }
}
