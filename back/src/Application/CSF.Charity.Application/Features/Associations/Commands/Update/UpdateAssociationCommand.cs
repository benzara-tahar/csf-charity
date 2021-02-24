using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Associations.Commands.Update
{
    public class UpdateAssociationCommand : IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public Guid TownshipId { get; set; }
        public Guid StateId { get; set; }

    }

    public class UpdateAssociationCommandHandler : IRequestHandler<UpdateAssociationCommand>
    {

        private readonly IAssociationRepository _repository;
        private readonly IStateRepository _stateRepository;
        private readonly ITownshipRepository _townshipRepository;
        private readonly IUnitOfWork _uow;

        public UpdateAssociationCommandHandler(IAssociationRepository repository,
            ITownshipRepository townshipRepository,
            IStateRepository stateRepository
, IUnitOfWork uow)
        {
            _repository = repository;
            _stateRepository = stateRepository;
            _townshipRepository = townshipRepository;
            _uow = uow;
        }

        public async Task<Unit> Handle(UpdateAssociationCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.GetById(request.Id);
            if (entity is null)
            {
                throw new NotFoundException(nameof(Association), request.Id);
            }
            var state = _stateRepository.GetById(request.StateId);
            if (state is null)
            {
                throw new NotFoundException(nameof(State), request.StateId);
            }
            var township = _townshipRepository.GetById(request.TownshipId);
            if (township is null)
            {
                throw new NotFoundException(nameof(Township), request.TownshipId);
            }
            entity.Name = request.Name;
            entity.NameLatin = request.NameLatin;
            entity.Description = request.Description;
            entity.State = state;
            entity.Township = township;

            _repository.Update(entity);
            await _uow.CommitAsync();
            return Unit.Value;
        }
    }
}
