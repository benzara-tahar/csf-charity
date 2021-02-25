using FluentValidation;

namespace CSF.Charity.Application.Allotments.Commands.Create
{
    public class CreateAllotmentCommandValidator : AbstractValidator<CreateAllotmentCommand>
    {
        public CreateAllotmentCommandValidator()
        {
            RuleFor(v => v.DonationDetails)
                .MinimumLength(10)
                .NotEmpty();

            RuleFor(v => v.CustomerId)
                .NotEmpty();

            RuleFor(v => v.AssociationId)
                .NotEmpty();

        }
    }
}
