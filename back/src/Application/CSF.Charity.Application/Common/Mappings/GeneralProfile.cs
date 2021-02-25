using AutoMapper;
using CSF.Charity.Application.Allotments.Commands.Create;
using CSF.Charity.Application.Allotments.Commands.Update;
using CSF.Charity.Application.Associations.Commands.Create;
using CSF.Charity.Application.Associations.Commands.Update;
using CSF.Charity.Application.Customers.Commands.Create;
using CSF.Charity.Application.Customers.Commands.Update;
using CSF.Charity.Application.Features.Allotments.DTOs;
using CSF.Charity.Application.Features.Associations.DTOs;
using CSF.Charity.Application.Features.Customers.DTOs;
using CSF.Charity.Application.Features.States.DTOs;
using CSF.Charity.Application.Features.Townships.DTOs;
using CSF.Charity.Application.States.Commands.Create;
using CSF.Charity.Application.States.Commands.Update;
using CSF.Charity.Application.Townships.Commands.Create;
using CSF.Charity.Application.Townships.Commands.Update;
using CSF.Charity.Domain.Entities;

namespace CSF.Charity.Application.Common.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            // association mapping
            this.CreateMap<CreateAssociationRequest, CreateAssociationCommand>();
            this.CreateMap<UpdateAssociationRequest, UpdateAssociationCommand>();
            this.CreateMap<Association, AssociationResponse>().ReverseMap();
            this.CreateMap<UpdateAssociationCommand, Association>();
            this.CreateMap<CreateAssociationCommand, Association>();
            // states
            this.CreateMap<CreateStateRequest, CreateStateCommand>();
            this.CreateMap<UpdateStateRequest, UpdateStateCommand>();
            this.CreateMap<State, StateResponse>().ReverseMap();
            this.CreateMap<UpdateStateCommand, State>();
            this.CreateMap<CreateStateCommand, State>();

            // townships
            this.CreateMap<CreateTownshipRequest, CreateTownshipCommand>();
            this.CreateMap<UpdateTownshipRequest, UpdateTownshipCommand>();
            this.CreateMap<Township, TownshipResponse>().ReverseMap();
            this.CreateMap<UpdateTownshipCommand, Township>();
            this.CreateMap<CreateTownshipCommand, Township>();
            // allotments

            this.CreateMap<CreateAllotmentRequest, CreateAllotmentCommand>();
            this.CreateMap<UpdateAllotmentRequest, UpdateAllotmentCommand>();
            this.CreateMap<UpdateAllotmentCommand, Allotment>();
            this.CreateMap<CreateAllotmentCommand, Allotment>();
            this.CreateMap<Allotment, AllotmentResponse>().ReverseMap();

            // customers
            this.CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
            this.CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();
            this.CreateMap<UpdateCustomerCommand, Customer>();
            this.CreateMap<CreateCustomerCommand, Customer>();
            this.CreateMap<Customer, CustomerResponse>().ReverseMap();
        }
    }
}