
using FluentValidation;

namespace CSF.Charity.Application.States.Commands.Update
{
    public class UpdateStateCommandValidator : AbstractValidator<UpdateStateCommand>
    {
        public UpdateStateCommandValidator()
        {
            RuleFor(v => v.Number)
             .GreaterThan(0)
             .LessThanOrEqualTo(100);

            RuleFor(v => v.Name)
             .MaximumLength(200)
             .NotEmpty();

            RuleFor(v => v.NameLatin)
                .MaximumLength(200)
                .NotEmpty();


        }
    }
}
