using AutoMapper;
using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Application.Features.Townships.DTOs;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Townships.Commands.Create
{

    public class CreateTownshipCommand : IRequest<Guid>, IMapFrom<CreateTownshipRequest>
    {
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public Guid StateId{ get; set; }
    }

    public class CreateTownshipCommandHandler : IRequestHandler<CreateTownshipCommand, Guid>
    {
        private readonly ITownshipRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateTownshipCommandHandler(ITownshipRepository repository,
            IMapper mapper, IUnitOfWork uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Guid> Handle(CreateTownshipCommand request, CancellationToken cancellationToken)
        {
            var entity = new Township
            {
                Name = request.Name,
                NameLatin = request.NameLatin,
                StateId = request.StateId,
            };

            _repository.Add(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);

        }
    }
}
