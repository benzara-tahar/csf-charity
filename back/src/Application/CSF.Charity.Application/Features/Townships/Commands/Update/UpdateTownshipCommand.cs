using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Townships.Commands.Update
{
    public class UpdateTownshipCommand : IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public Guid StateId { get; set; }

    }

    public class UpdateTownshipCommandHandler : IRequestHandler<UpdateTownshipCommand>
    {

        private readonly ITownshipRepository _repository;
        private readonly IUnitOfWork _uow;

        public UpdateTownshipCommandHandler(ITownshipRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(UpdateTownshipCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.GetById(request.Id);
            if (entity is null)
            {
                throw new NotFoundException(nameof(Township), request.Id);
            }
          
            entity.Name = request.Name;
            entity.NameLatin = request.NameLatin;
            entity.StateId = request.StateId;

            _repository.Update(entity);
            await _uow.CommitAsync();
            return Unit.Value;
        }
    }
}
