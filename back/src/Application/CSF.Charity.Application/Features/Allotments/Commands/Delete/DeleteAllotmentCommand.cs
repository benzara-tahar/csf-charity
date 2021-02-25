using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Allotments.Commands.Delete
{

    public class DeleteAllotmentCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteAllotmentCommandHandler : IRequestHandler<DeleteAllotmentCommand>
    {
        private readonly IAllotmentRepository _repository;
        private readonly IUnitOfWork _uow;

        public DeleteAllotmentCommandHandler(IAllotmentRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(DeleteAllotmentCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.GetById(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Allotment), request.Id);
            }

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return await Task.FromResult(Unit.Value);
        }
    }
}
