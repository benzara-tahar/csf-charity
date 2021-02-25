using AutoMapper;
using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Allotments.Commands.Update
{
    public class UpdateAllotmentCommand : IRequest<Guid>
    {

        public Guid Id { get; set; }
        public Guid AssociationId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTimeOffset Date { get; set; }
        public string DonationDetails { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateAllotmentCommandHandler : IRequestHandler<UpdateAllotmentCommand, Guid>
    {

        private readonly IAllotmentRepository _repository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAssociationRepository _associationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public UpdateAllotmentCommandHandler(IAllotmentRepository repository,
            ICustomerRepository customerRepository,
            IAssociationRepository associationRepository,
            IMapper mapper, IUnitOfWork uow)
        {
            _repository = repository;
            _associationRepository = associationRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<Guid> Handle(UpdateAllotmentCommand request, CancellationToken cancellationToken)
        {

            var entity = _repository.GetById(request.Id);
            if (entity is null)
            {
                throw new NotFoundException(nameof(Allotment), request.Id);
            }
            var customer = _customerRepository.GetById(request.CustomerId);
            if (customer is null)
            {
                throw new NotFoundException(nameof(Customer), request.CustomerId);
            }
            var association = _associationRepository.GetById(request.AssociationId);
            if (association is null)
            {
                throw new NotFoundException(nameof(Association), request.AssociationId);
            }
            entity.AssociationId = request.AssociationId;
            entity.CustomerId = request.CustomerId;
            entity.Date = request.Date;
            entity.DonationDetails = request.DonationDetails;
            entity.Notes = request.Notes;


            _repository.Update(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);
        }
    }
}
