using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSF.Charity.Application.Common.Interfaces;
using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Application.Common.Models;
using CSF.Charity.Application.TodoItems.Repositories;
using CSF.Charity.Application.TodoLists.Queries.GetTodos;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    public class GetTodoItemsWithPaginationQuery : IRequest<PaginatedList<TodoItemDto>>
    {
        public int ListId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemDto>>
    {
        private readonly ITodoItemRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoItemsWithPaginationQueryHandler(ITodoItemRepository context, IMapper mapper)
        {
            _repository = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var result = _repository
                .GetAllPaged(request.PageNumber, request.PageSize, x => x.ListId == request.ListId);
            // TODO! add ordering
            //.OrderBy(x => x.Title)
            //.ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            var dto = _mapper.Map<IEnumerable<TodoItemDto>>(result);
            var page = new PaginatedList<TodoItemDto>(dto, 100, request.PageNumber, request.PageSize);
            return await Task.FromResult(page);

        }
    }
}
