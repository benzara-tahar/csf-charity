using CSF.Charity.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;

namespace CSF.Charity.Application.Features.Customers.DTOs
{
    public class CreateCustomerRequest 
    {

        public string Firstname { get; set; }
        public string FirstnameLatin { get; set; }
        public string Lastname { get; set; }
        public string LastnameLatin { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthPlaceLatin { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdCardNumber { get; set; }
        public Guid TownshipId { get; set; }
        public Guid StateId { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile IllnessCertificationPhoto { get; set; }
        public FamilliarSituation FamilliarSituation { get; set; }
        public string ExtraInformation { get; set; }

    }
}
