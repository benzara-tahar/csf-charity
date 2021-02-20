using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Domain.Entities;

namespace CSF.Charity.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
