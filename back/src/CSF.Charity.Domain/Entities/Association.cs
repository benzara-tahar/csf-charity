using CSF.Charity.Domain.Common;
using CSF.Charity.Domain.Enums;
using CSF.Charity.Domain.Events;
using System;
using System.Collections.Generic;

namespace CSF.Charity.Domain.Entities
{
    public class Association : AuditableEntity<Guid>
    {

        public int Name { get; set; }
        public int NameLatin { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; set; }

        public DateTime? Reminder { get; set; }

      
    }
}
