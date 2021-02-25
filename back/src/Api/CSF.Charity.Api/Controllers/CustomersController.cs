using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using CSF.Charity.Application.Customers.Commands.Create;
using CSF.Charity.Application.Customers.Commands.Delete;
using CSF.Charity.Application.Customers.Commands.Update;
using CSF.Charity.Application.Common.Models;
using CSF.Charity.Application.Features.Customers.DTOs;
using CSF.Charity.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CSF.Charity.Application.TodoLists.Queries.GetTodos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CSF.Charity.Application.Services;

namespace CSF.Charity.Api.Controllers
{

    //[Authorize]
    /// <summary>
    /// customers endpoints
    /// </summary>
    public class CustomersController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public CustomersController(IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// creat a new customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromForm]CreateCustomerRequest request)
        {
            var command = _mapper.Map<CreateCustomerCommand>(request);
            command.Photo = _photoService.ConvertToBase64String(request.Photo);
            command.IllnessCertificationPhoto = _photoService.ConvertToBase64String(request.IllnessCertificationPhoto);
            return await Mediator.Send(command);
        }

        /// <summary>
        /// update a customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromForm] UpdateCustomerRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var command = _mapper.Map<UpdateCustomerCommand>(request);
            command.Photo = _photoService.ConvertToBase64String(request.Photo);
            command.IllnessCertificationPhoto = _photoService.ConvertToBase64String(request.IllnessCertificationPhoto);
            await Mediator.Send(_mapper.Map<UpdateCustomerCommand>(command));

            return Ok();
        }


        /// <summary>
        /// delete an existing customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteCustomerCommand { Id = id });
            return Ok();
        }
    }
}
