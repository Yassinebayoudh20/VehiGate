using VehiGate.Application.Authentication.Commands.UpdateUserInfo;

namespace VehiGate.Application.Authentication.Commands.Register;

public class UpdateUserInfoCommandValidator : AbstractValidator<UpdateUserInfoCommand>
{
    public UpdateUserInfoCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("INVALID_EMAIL_FORMAT");
    }
}
