

using AspNetCore.Identity.Mongo.Model;
using CSF.Charity.Domain.Core.Models;
using MongoDB.Bson;
using System;

namespace CSF.Charity.Domain.Identity
{
    public class ApplicationUser :  MongoUser, IEntity<ObjectId>
    {
       
        public string AssociationId { get; set; }

        public DateTime? LastLoggedIn { get; set; } = null;
        public DateTime LastLoggedOut { get; set; }
        public DateTime AddedAtUtc { get; set; }
        public string FirstName { get;  set; }
        public string LastName { get; set; }

        public ApplicationUser()
            : base()
        {
        }

    }
}
