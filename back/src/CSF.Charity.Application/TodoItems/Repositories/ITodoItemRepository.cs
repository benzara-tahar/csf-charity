using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSF.Charity.Application.Common;
using CSF.Charity.Domain.Entities;
using CSF.Charity.Application.Common.Interfaces;

namespace CSF.Charity.Application.TodoItems.Repositories
{
    public interface ITodoItemRepository: IRepository<TodoItem,Guid>
    {
    }
}
