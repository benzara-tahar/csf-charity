using AutoMapper;
using CSF.Charity.Application.Features.States.DTOs;
using CSF.Charity.Application.Features.Townships.DTOs;
using CSF.Charity.Application.States.Commands.Create;
using CSF.Charity.Application.States.Commands.Delete;
using CSF.Charity.Application.States.Commands.Update;
using CSF.Charity.Application.Townships.Commands.Create;
using CSF.Charity.Application.Townships.Commands.Delete;
using CSF.Charity.Application.Townships.Commands.Update;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSF.Charity.Api.Controllers
{
    //[Authorize]
    public class ReferencialsController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public ReferencialsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
        //{
        //    return await Mediator.Send(query);
        //}

        [HttpPost("states")]
        public async Task<ActionResult<Guid>> CreateState(CreateStateRequest request)
        {
            return await Mediator.Send(_mapper.Map<CreateStateCommand>(request));
        }

        [HttpPut("states/{id}")]
        public async Task<ActionResult> UpdateState(Guid id, UpdateStateRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(_mapper.Map<UpdateStateCommand>(request));

            return Ok();
        }



        [HttpDelete("states/{id}")]
        public async Task<ActionResult> DeleteState(Guid id)
        {
            await Mediator.Send(new DeleteStateCommand { Id = id });

            return Ok();
        }



        [HttpPost("townships")]
        public async Task<ActionResult<Guid>> CreateTownship(CreateTownshipRequest request)
        {
            return await Mediator.Send(_mapper.Map<CreateTownshipCommand>(request));
        }

        [HttpPut("townships/{id}")]
        public async Task<ActionResult> UpdateTownship(Guid id, UpdateTownshipRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(_mapper.Map<UpdateTownshipCommand>(request));

            return Ok();
        }



        [HttpDelete("townships/{id}")]
        public async Task<ActionResult> DeleteTownship(Guid id)
        {
            await Mediator.Send(new DeleteTownshipCommand { Id = id });

            return Ok();
        }
    }
}
