using AutoMapper;
using CSF.Charity.Application.Associations.Commands.Create;
using CSF.Charity.Application.Associations.Commands.Delete;
using CSF.Charity.Application.Associations.Commands.Update;
using CSF.Charity.Application.Common.Models;
using CSF.Charity.Application.Features.Associations.DTOs;
using CSF.Charity.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CSF.Charity.Application.TodoLists.Queries.GetTodos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSF.Charity.Api.Controllers
{
    //[Authorize]
    public class AssociationsController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public AssociationsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateAssociationRequest request)
        {
            return await Mediator.Send(_mapper.Map<CreateAssociationCommand>(request));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateAssociationRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(_mapper.Map<UpdateAssociationCommand>(request));

            return Ok();
        }

      

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteAssociationCommand { Id = id });

            return Ok();
        }
    }
}
