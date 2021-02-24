using AutoMapper;
using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Application.Features.States.DTOs;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.States.Commands.Create
{

    public class CreateStateCommand : IRequest<Guid>, IMapFrom<CreateStateRequest>
    {
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public int Number{ get; set; }
    }

    public class CreateStateCommandHandler : IRequestHandler<CreateStateCommand, Guid>
    {
        private readonly IStateRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateStateCommandHandler(IStateRepository repository,
            IMapper mapper, IUnitOfWork uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Guid> Handle(CreateStateCommand request, CancellationToken cancellationToken)
        {
            var entity = new State
            {
                Name = request.Name,
                NameLatin = request.NameLatin,
                Number = request.Number,
            };

            _repository.Add(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);

        }
    }
}
