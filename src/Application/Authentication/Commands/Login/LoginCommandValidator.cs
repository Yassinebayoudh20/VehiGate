namespace VehiGate.Application.Authentication.Commands.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("EMAIL_REQUIRED")
            .EmailAddress()
            .WithMessage("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("PASSWORD_REQUIRED");
    }
}
