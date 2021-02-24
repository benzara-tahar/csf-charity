using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using System;

namespace CSF.Charity.Infrastructure.Persistance.Mongo.Repositories
{
    public class AssociationRepository : MongoRepository<Association, Guid>, IAssociationRepository
    {
        public AssociationRepository(IMongoContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
