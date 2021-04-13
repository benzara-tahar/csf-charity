using CSF.Charity.Application.Features.Users.DTOs;
using CSF.Charity.Domain.Identity;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Charity.Application.Features.Users.Mappers
{
    public static class UserMapperExtensions
    {
        public static UserDetailsResponse ToUserDetailsDto(this ApplicationUser e, List<ApplicationRole> roles)
        {
            var userRole = roles.FirstOrDefault(u => u.Id.ToString() == e.Roles.FirstOrDefault());
            return new UserDetailsResponse
            {
                Id = e.Id.ToString(),
                Email = e.Email,
                UserName = e.UserName,
                Role = userRole.Name ,
                AssociationId = e.AssociationId
            };
        }

        public static LoginResponse ToUserLoggedInDto(this ApplicationUser e, List<ApplicationRole> roles, string token)
        {
            var userRole = roles.FirstOrDefault(u => u.Id.ToString() == e.Roles.FirstOrDefault());
            return new LoginResponse
            {
                Id = e.Id.ToString(),
                Email = e.Email,
                UserName = e.UserName,
                Role = userRole.Name,
                AssociationId = e.AssociationId,
                Token = token
            };
        }
    }
}
