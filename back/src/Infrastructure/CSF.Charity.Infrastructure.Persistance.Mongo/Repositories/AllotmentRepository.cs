using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using System;

namespace CSF.Charity.Infrastructure.Persistance.Mongo.Repositories
{
    public class AllotmentRepository : MongoRepository<Allotment, Guid>, IAllotmentRepository
    {
        public AllotmentRepository(IMongoContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
