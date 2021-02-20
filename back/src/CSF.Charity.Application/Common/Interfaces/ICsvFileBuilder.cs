using CSF.Charity.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace CSF.Charity.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
