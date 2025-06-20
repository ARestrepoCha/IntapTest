using FluentValidation;
using IntapTest.Shared.Dtos.Requests;

namespace IntapTest.Domain.Validator
{
    public class CreateActivityValidator : AbstractValidator<CreateActivityRequestDto>
    {
        public CreateActivityValidator()
        {
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es requerida.")
                .NotNull().WithMessage("La descripción es requerida.");
        }
    }
}
