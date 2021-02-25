﻿using CSF.Charity.Domain.Core.Models;
using CSF.Charity.Domain.Enums;
using System;

namespace CSF.Charity.Domain.Entities
{
    public class Customer : Entity<Guid>
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
        public Township Township{ get; set; }
        public State State { get; set; }

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
