using System;

namespace CSF.Charity.Application.Features.Townships.DTOs
{
    public class TownshipResponse
    {
        public Guid Id { get; set; }
        public Guid StateId { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        
    
    }
}
