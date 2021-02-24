using FluentValidation;

namespace CSF.Charity.Application.Associations.Commands.Create
{
    public class CreateAssociationCommandValidator : AbstractValidator<CreateAssociationCommand>
    {
        public CreateAssociationCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.NameLatin)
                .MaximumLength(200)
                .NotEmpty();


            RuleFor(v => v.TownshipId)
                .NotEmpty();

            RuleFor(v => v.StateId)
                .NotEmpty();

        }
    }
}
