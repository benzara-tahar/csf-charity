using System;

namespace CSF.Charity.Application.Features.Allotments.DTOs
{
    public class CreateAllotmentRequest 
    {

        public Guid AssociationId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTimeOffset Date { get; set; }
        public string DonationDetails { get; set; }
        public string Notes { get; set; }
    }
}
