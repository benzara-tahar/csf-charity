using System;

namespace CSF.Charity.Domain.Core.Models
{
    public abstract class AuditableEntity<TKey>: Entity<TKey>
    {
        
        public TKey Id { get; set; }
        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
        
    }

    public abstract class AuditableEntity : AuditableEntity<string>
    {

    }
}
