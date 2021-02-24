using FluentValidation;

namespace CSF.Charity.Application.Townships.Commands.Create
{
    public class CreateTownshipCommandValidator : AbstractValidator<CreateTownshipCommand>
    {
        public CreateTownshipCommandValidator()
        {
            RuleFor(v => v.StateId)
                .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.NameLatin)
                .MaximumLength(200)
                .NotEmpty();

            // TODO! add state id verification

        }
    }
}
