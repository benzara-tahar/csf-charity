using System;

namespace CSF.Charity.Application.Features.Townships.DTOs
{
    public class UpdateTownshipRequest
    {

        public Guid Id { get; set; }
        public Guid StateId { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        
    }
}
