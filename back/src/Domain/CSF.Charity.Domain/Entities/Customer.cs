using CSF.Charity.Domain.Core.Models;
using CSF.Charity.Domain.Enums;
using System;

namespace CSF.Charity.Domain.Entities
{
    public class Customer : AuditableEntity<Guid>
    {
        public int Firstname { get; set; }
        public int FirstnameLatin { get; set; }
        public int Lastname { get; set; }
        public int LastnameLatin { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthPlaceLatin { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdCardNumber { get; set; }
        /// <summary>
        /// base64 photo
        /// </summary>
        public string Photo{ get; set; }
        /// <summary>
        /// base64 photo
        /// </summary>
        public string IllnessCertificationPhoto { get; set; }
        public FamilliarSituation FamilliarSituation { get; set; }
        public string ExtraInformation { get; set; }

    }
}
