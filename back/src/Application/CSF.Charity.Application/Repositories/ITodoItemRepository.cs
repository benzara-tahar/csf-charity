using System;
using CSF.Charity.Domain.Entities;
using CSF.Charity.Application.Common.Abstractions;

namespace CSF.Charity.Application.Repositories
{
    public interface ITodoItemRepository : IRepository<TodoItem, Guid>
    {
    }
}
