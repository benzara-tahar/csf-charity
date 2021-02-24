using CSF.Charity.Domain.Core.Models;
using System;

namespace CSF.Charity.Domain.Entities
{
    public class Association : Entity<Guid>
    {

        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public Township Township { get; set; }
        public State State { get; set; }


    }
}
