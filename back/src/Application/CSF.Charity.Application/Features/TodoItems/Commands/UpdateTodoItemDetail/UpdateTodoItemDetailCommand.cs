using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using CSF.Charity.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.TodoItems.Commands.UpdateTodoItemDetail
{
    public class UpdateTodoItemDetailCommand : IRequest
    {
        public Guid Id { get; set; }

        public int ListId { get; set; }

        public PriorityLevel Priority { get; set; }

        public string Note { get; set; }
    }

    public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
    {
        private readonly ITodoItemRepository _repository;
        public UpdateTodoItemDetailCommandHandler(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            var entity =  _repository.GetById(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            entity.ListId = request.ListId;
            entity.Priority = request.Priority;
            entity.Note = request.Note;

            _repository.Update(entity/*cancellationToken*/);

            return await Task.FromResult(Unit.Value);
        }
    }
}
