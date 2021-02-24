using AutoMapper;
using CSF.Charity.Application.Associations.Commands.Create;
using CSF.Charity.Application.Associations.Commands.Update;
using CSF.Charity.Application.Features.Associations.DTOs;
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

            // states
            this.CreateMap<CreateStateRequest, CreateStateCommand>();
            this.CreateMap<UpdateStateRequest, UpdateStateCommand>();
            this.CreateMap<State, StateResponse>().ReverseMap();


            // townships
            this.CreateMap<CreateTownshipRequest, CreateTownshipCommand>();
            this.CreateMap<UpdateTownshipRequest, UpdateTownshipCommand>();
            this.CreateMap<Township, TownshipResponse>().ReverseMap();

            // allotments

            //customers
        }
    }
}