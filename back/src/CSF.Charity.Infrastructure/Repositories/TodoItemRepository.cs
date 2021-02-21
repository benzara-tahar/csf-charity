using CSF.Charity.Application.TodoItems.Repositories;
using CSF.Charity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSF.Charity.Infrastructure.Repositories
{
    public class TodoItemRepository : MongoRepository<TodoItem, Guid>, ITodoItemRepository
    {
        public TodoItemRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
