using System;

namespace CSF.Charity.Application.Features.States.DTOs
{
    public class StateResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        
    
    }
}
