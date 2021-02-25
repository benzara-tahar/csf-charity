
using FluentValidation;

namespace CSF.Charity.Application.Allotments.Commands.Update
{
    public class UpdateAllotmentCommandValidator : AbstractValidator<UpdateAllotmentCommand>
    {
        public UpdateAllotmentCommandValidator()
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
