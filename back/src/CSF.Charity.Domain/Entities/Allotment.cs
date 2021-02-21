using CSF.Charity.Domain.Common;
using CSF.Charity.Domain.Enums;
using CSF.Charity.Domain.Events;
using System;
using System.Collections.Generic;

namespace CSF.Charity.Domain.Entities
{
    public class Allotment : AuditableEntity<Guid>
    {
        public Guid CustomerId { get; set; }

        public DateTimeOffset Date{ get; set; }
        public string DonationDetails { get; set; }
        public string Notes { get; set; }

    }
}
