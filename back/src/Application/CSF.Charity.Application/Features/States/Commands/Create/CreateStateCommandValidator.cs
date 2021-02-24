using FluentValidation;

namespace CSF.Charity.Application.States.Commands.Create
{
    public class CreateStateCommandValidator : AbstractValidator<CreateStateCommand>
    {
        public CreateStateCommandValidator()
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

            // TODO! add unicity validation here

        }
    }
}
