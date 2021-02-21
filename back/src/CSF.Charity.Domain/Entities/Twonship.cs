using CSF.Charity.Domain.Common;
using CSF.Charity.Domain.Enums;
using CSF.Charity.Domain.Events;
using System;
using System.Collections.Generic;

namespace CSF.Charity.Domain.Entities
{
    public class Township : AuditableEntity<Guid>
    {
        public int StateId { get; set; }
        public int Name{ get; set; }
        public int NameLatin { get; set; }
    }
}
