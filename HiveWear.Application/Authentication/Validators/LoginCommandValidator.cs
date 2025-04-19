using FluentValidation;
using HiveWear.Application.Authentication.Commands;

namespace HiveWear.Application.Authentication.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginModel)
                .NotNull()
                .WithMessage("Login model is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.LoginModel!.Email)
                        .NotEmpty()
                        .WithMessage("Email is required.")
                        .EmailAddress()
                        .WithMessage("Invalid email format.");
                    RuleFor(x => x.LoginModel!.Password)
                        .NotEmpty()
                        .WithMessage("Password is required.");
                });
        }
    }
}
