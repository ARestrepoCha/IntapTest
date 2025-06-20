using FluentValidation;
using IntapTest.Shared.Dtos.Requests;

namespace IntapTest.Domain.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
    {
        public CreateUserValidator()
        {
            RuleFor(CreateUserRequestDto => CreateUserRequestDto.Email).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Usuario es requerido")
                .NotEmpty().WithMessage("Usuario es requerido")
                .EmailAddress().WithMessage("Email con formato incorrecto");

            RuleFor(CreateUserRequestDto => CreateUserRequestDto.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Contraseña es requerida")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!.,;:_+%*?&^'\s-].*)[A-Za-z\d@$!%*.,;:_+?&^'\s-]{8,16}$")
                .WithMessage("Contraseña no cumple con las politicas de seguridad")
                .Equal(dto => dto.PasswordConfirmation).WithMessage("La contraseña y su confirmación deben ser iguales");

            RuleFor(CreateUserRequestDto => CreateUserRequestDto.FullName).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Nombre es requerido")
                .NotEmpty().WithMessage("Nombre es requerido");
        }
    }
}
