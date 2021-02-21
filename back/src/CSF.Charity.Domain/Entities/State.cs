using CSF.Charity.Domain.Common;
using CSF.Charity.Domain.Enums;
using CSF.Charity.Domain.Events;
using System;
using System.Collections.Generic;

namespace CSF.Charity.Domain.Entities
{
    public class State : AuditableEntity<int>
    {
        public int Name { get; set; }
        public int NameLatin { get; set; }
    }
}
