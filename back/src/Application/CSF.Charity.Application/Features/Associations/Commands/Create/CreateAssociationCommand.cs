using AutoMapper;
using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Application.Features.Associations.DTOs;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Associations.Commands.Create
{

    public class CreateAssociationCommand : IRequest<Guid>, IMapFrom<CreateAssociationRequest>
    {
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string Description { get; set; }
        public Guid TownshipId { get; set; }
        public Guid StateId { get; set; }
    }

    public class CreateAssociationCommandHandler : IRequestHandler<CreateAssociationCommand, Guid>
    {
        private readonly IAssociationRepository _repository;
        private readonly IStateRepository _stateRepository;
        private readonly ITownshipRepository _townshipRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateAssociationCommandHandler(IAssociationRepository repository,
            ITownshipRepository townshipRepository,
            IStateRepository stateRepository,
            IMapper mapper, IUnitOfWork uow)
        {
            _repository = repository;
            _stateRepository = stateRepository;
            _townshipRepository = townshipRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Guid> Handle(CreateAssociationCommand request, CancellationToken cancellationToken)
        {
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
            var entity = new Association
            {
                Name = request.Name,
                NameLatin = request.NameLatin,
                Description = request.Description,
                State = state,
                Township = township,
            };

            _repository.Add(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);

        }
    }
}
