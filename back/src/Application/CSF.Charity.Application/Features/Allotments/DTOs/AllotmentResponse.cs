using System;

namespace CSF.Charity.Application.Features.Allotments.DTOs
{
    public class AllotmentResponse
    {
        public Guid Id { get; set; }
        public Guid AssociationId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTimeOffset Date { get; set; }
        public string DonationDetails { get; set; }
        public string Notes { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
     
    }
}
