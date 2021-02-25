using AutoMapper;
using CSF.Charity.Application.Allotments.Commands.Create;
using CSF.Charity.Application.Allotments.Commands.Delete;
using CSF.Charity.Application.Allotments.Commands.Update;
using CSF.Charity.Application.Common.Models;
using CSF.Charity.Application.Features.Allotments.DTOs;
using CSF.Charity.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CSF.Charity.Application.TodoLists.Queries.GetTodos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSF.Charity.Api.Controllers
{
    //[Authorize]
    public class AllotmentsController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public AllotmentsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateAllotmentRequest request)
        {
            return await Mediator.Send(_mapper.Map<CreateAllotmentCommand>(request));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateAllotmentRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(_mapper.Map<UpdateAllotmentCommand>(request));

            return Ok();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteAllotmentCommand { Id = id });

            return Ok();
        }
    }
}
