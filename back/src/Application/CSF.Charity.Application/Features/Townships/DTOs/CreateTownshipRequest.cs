using System;

namespace CSF.Charity.Application.Features.Townships.DTOs
{
    public class CreateTownshipRequest 
    {

        public string Name { get; set; }
        public string NameLatin { get; set; }
        public Guid StateId { get; set; }

    }
}
