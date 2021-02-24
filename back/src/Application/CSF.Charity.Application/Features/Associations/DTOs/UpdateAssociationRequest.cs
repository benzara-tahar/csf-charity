using System;

namespace CSF.Charity.Application.Features.Associations.DTOs
{
    public class UpdateAssociationRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public Guid TownshipId { get; set; }
        public Guid StateId { get; set; }
    }
}
