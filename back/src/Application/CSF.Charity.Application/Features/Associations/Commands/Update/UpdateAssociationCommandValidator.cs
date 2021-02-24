
using FluentValidation;

namespace CSF.Charity.Application.Associations.Commands.Update
{
    public class UpdateAssociationCommandValidator : AbstractValidator<UpdateAssociationCommand>
    {
        public UpdateAssociationCommandValidator()
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
