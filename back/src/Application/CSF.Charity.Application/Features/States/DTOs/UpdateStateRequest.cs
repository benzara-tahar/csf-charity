using System;

namespace CSF.Charity.Application.Features.States.DTOs
{
    public class UpdateStateRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public int Number { get; set; }
    }
}
