using System;

namespace CSF.Charity.Application.Features.Associations.DTOs
{
    public class AssociationResponse
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public Guid TownshipId { get; set; }
        public Guid StateId { get; set; }
    }
}
