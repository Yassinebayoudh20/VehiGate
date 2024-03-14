namespace VehiGate.Application.Authentication.Commands.Register;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
             .NotEmpty()
             .WithMessage("PHONE_NUMBER_REQUIRED");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("EMAIL_REQUIRED")
            .EmailAddress()
            .WithMessage("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("PASSWORD_REQUIRED")
               .MinimumLength(6)
               .WithMessage("PASSWORD_MIN_LENGTH")
               .Matches("[A-Z]")
               .WithMessage("PASSWORD_UPPERCASE_LETTER_REQUIRED")
               .Matches("[a-z]")
               .WithMessage("PASSWORD_LOWERCASE_LETTER_REQUIRED")
               .Matches("[0-9]")
               .WithMessage("PASSWORD_DIGIT_REQUIRED")
               .Matches("[^a-zA-Z0-9]")
               .WithMessage("PASSWORD_SPECIAL_CHARACTER_REQUIRED");

        RuleFor(x => x.Roles)
            .Must(roles => roles != null && roles.Count > 0)
            .When(x => x.Roles != null)
            .WithMessage("ROLES_REQUIRED");
    }
}
