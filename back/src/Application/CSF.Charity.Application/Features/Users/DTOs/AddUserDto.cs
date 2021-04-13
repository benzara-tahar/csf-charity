using FluentValidation;
using System.Collections.Generic;

namespace CSF.Charity.Application.Features.Users.DTOs
{
    public class AddUserDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
        public string Role { get; set; }
        public string AssociationId { get; set; }
    }
    public class AddUserDtoValidator : AbstractValidator<AddUserDto>
    {
        public AddUserDtoValidator()
        {
            RuleFor(o => o.UserName).NotEmpty();
            RuleFor(o => o.Password).NotEmpty();
            RuleFor(o => o.Email).NotEmpty();
            RuleFor(o => o.Role).NotEmpty();
        }
    }
}
