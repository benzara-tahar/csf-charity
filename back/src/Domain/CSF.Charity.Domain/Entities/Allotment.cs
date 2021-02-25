﻿using CSF.Charity.Domain.Core.Models;
using System;

namespace CSF.Charity.Domain.Entities
{
    public class Allotment : Entity<Guid>
    {
        public Guid AssociationId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTimeOffset Date{ get; set; }
        public string DonationDetails { get; set; }
        public string Notes { get; set; }

    }
}
