﻿using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using CSF.Charity.Domain.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.TodoItems.Commands.CreateTodoItem
{
    public class CreateTodoItemCommand : IRequest<Guid>
    {
        public int ListId { get; set; }

        public string Title { get; set; }
    }

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Guid>
    {
        private readonly ITodoItemRepository _repository;
        private readonly IUnitOfWork _uow;

        public CreateTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoItem
            {
                ListId = request.ListId,
                Title = request.Title,
                Done = false
            };

            entity.DomainEvents.Add(new TodoItemCreatedEvent(entity));
            // TODO! support cancelation token in the repository
            _repository.Add(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);
            
        }
    }
}