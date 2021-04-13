using FluentValidation;

namespace CSF.Charity.Application.Features.Users.DTOs
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string TenantId { get; set; }
    }

    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(o => o.UserName).NotEmpty();
            RuleFor(o => o.Email).NotEmpty().EmailAddress();
            RuleFor(o => o.Role).NotEmpty();
        }
    }
}
