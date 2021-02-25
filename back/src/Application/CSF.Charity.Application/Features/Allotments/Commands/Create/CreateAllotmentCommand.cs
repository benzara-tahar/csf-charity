using AutoMapper;
using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Application.Features.Allotments.DTOs;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Allotments.Commands.Create
{

    public class CreateAllotmentCommand : IRequest<Guid>, IMapFrom<CreateAllotmentRequest>
    {
        public Guid AssociationId { get; set; }
        public Guid CustomerId { get; set; }

        public DateTimeOffset Date { get; set; }
        public string DonationDetails { get; set; }
        public string Notes { get; set; }
    }

    public class CreateAllotmentCommandHandler : IRequestHandler<CreateAllotmentCommand, Guid>
    {
        private readonly IAllotmentRepository _repository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAssociationRepository _associationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateAllotmentCommandHandler(IAllotmentRepository repository,
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

        public async Task<Guid> Handle(CreateAllotmentCommand request, CancellationToken cancellationToken)
        {
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
            var entity = new Allotment
            {
                AssociationId = request.AssociationId,
                CustomerId= request.CustomerId,
                Date= request.Date,
                DonationDetails = request.DonationDetails,
                Notes = request.Notes,
            };

            _repository.Add(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);

        }
    }
}
