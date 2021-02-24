namespace CSF.Charity.Domain.Core.Models
{
    public abstract class Entity<TKey>: IEntity<TKey>
    {
        
        public TKey Id { get; set; }
        
    }

    public abstract class Entity : Entity<string>
    {

    }
}
