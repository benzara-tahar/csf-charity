using AutoMapper;
using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Application.Common.Exceptions;
using CSF.Charity.Application.Common.Mappings;
using CSF.Charity.Application.Features.Customers.DTOs;
using CSF.Charity.Application.Repositories;
using CSF.Charity.Domain.Entities;
using CSF.Charity.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Customers.Commands.Create
{

    public class CreateCustomerCommand : IRequest<Guid>, IMapFrom<CreateCustomerRequest>
    {

        public string Firstname { get; set; }
        public string FirstnameLatin { get; set; }
        public string Lastname { get; set; }
        public string LastnameLatin { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthPlaceLatin { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdCardNumber { get; set; }
        public Guid TownshipId { get; set; }
        public Guid StateId { get; set; }
        /// <summary>
        /// base64 photo
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// base64 photo
        /// </summary>
        public string IllnessCertificationPhoto { get; set; }
        public FamilliarSituation FamilliarSituation { get; set; }
        public string ExtraInformation { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repository;
        private readonly IStateRepository _stateRepository;
        private readonly ITownshipRepository _townshipRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateCustomerCommandHandler(ICustomerRepository repository,
            IMapper mapper,
            IUnitOfWork uow,
            IStateRepository stateRepository,
            ITownshipRepository townshipRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
            _stateRepository = stateRepository;
            _townshipRepository = townshipRepository;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Customer>(request);
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
            entity.Township = township;
            entity.State = state;

            // TODO mapper vs manuall 
            //entity.Firstname = request.Firstname;
            //entity.FirstnameLatin = request.FirstnameLatin;
            //entity.Lastname = request.Lastname;
            //entity.LastnameLatin = request.LastnameLatin;
            //entity.BirthDate = request.BirthDate;
            //entity.BirthPlace = request.BirthPlace;
            //entity.BirthPlaceLatin = request.BirthPlaceLatin;
            //entity.Email = request.Email;
            //entity.PhoneNumber = request.PhoneNumber;

            //entity.IdCardNumber = request.IdCardNumber;
            //entity.Photo = request.Photo;
            //entity.IllnessCertificationPhoto = request.IllnessCertificationPhoto;
            //entity.FamilliarSituation = request.FamilliarSituation;
            //entity.ExtraInformation = request.ExtraInformation;

            _repository.Add(entity);
            await _uow.CommitAsync();
            return await Task.FromResult(entity.Id);

        }
    }
}
