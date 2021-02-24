using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.States.Commands.Update
{
    public class UpdateStateCommand : IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public int Number { get; set; }

    }

    public class UpdateStateCommandHandler : IRequestHandler<UpdateStateCommand>
    {

        private readonly IStateRepository _repository;
        private readonly IUnitOfWork _uow;

        public UpdateStateCommandHandler(IStateRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(UpdateStateCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.GetById(request.Id);
            if (entity is null)
            {
                throw new NotFoundException(nameof(State), request.Id);
            }
          
            entity.Name = request.Name;
            entity.NameLatin = request.NameLatin;
            entity.Number = request.Number;

            _repository.Update(entity);
            await _uow.CommitAsync();
            return Unit.Value;
        }
    }
}
