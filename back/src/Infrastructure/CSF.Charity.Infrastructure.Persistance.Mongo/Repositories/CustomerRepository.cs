using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using System;

namespace CSF.Charity.Infrastructure.Persistance.Mongo.Repositories
{
    public class CustomerRepository : MongoRepository<Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(IMongoContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
