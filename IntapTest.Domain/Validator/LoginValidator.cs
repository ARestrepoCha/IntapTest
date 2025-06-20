using FluentValidation;
using IntapTest.Shared.Dtos.Requests;

namespace IntapTest.Domain.Validator
{
    public class LoginValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginValidator()
        {
            RuleFor(loginUserDto => loginUserDto.UserName).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Usuario es requerido")
                .NotEmpty().WithMessage("Usuario es requerido")
                .EmailAddress().WithMessage("Email incorrecto");

            RuleFor(loginUserDto => loginUserDto.Password).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Contraseña es requerida")
                .NotEmpty().WithMessage("Contraseña es requerida");
        }
    }
}
