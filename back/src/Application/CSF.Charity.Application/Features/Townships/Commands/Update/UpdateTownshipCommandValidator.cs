
using FluentValidation;

namespace CSF.Charity.Application.Townships.Commands.Update
{
    public class UpdateTownshipCommandValidator : AbstractValidator<UpdateTownshipCommand>
    {
        public UpdateTownshipCommandValidator()
        {
            RuleFor(v => v.StateId)
             .NotEmpty();

            RuleFor(v => v.Name)
             .MaximumLength(200)
             .NotEmpty();

            RuleFor(v => v.NameLatin)
                .MaximumLength(200)
                .NotEmpty();


        }
    }
}
