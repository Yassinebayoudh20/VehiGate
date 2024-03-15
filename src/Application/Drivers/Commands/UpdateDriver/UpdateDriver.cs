using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Users.Commands.UpdateUserInfo;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Drivers.Commands.UpdateDriver
{
    [Authorize]
    public record UpdateDriverCommand : IRequest<Unit>
    {
        public string Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string DriverLicenseNumber { get; init; }
    }

    public class UpdateDriverCommandValidator : AbstractValidator<UpdateDriverCommand>
    {
        public UpdateDriverCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).MaximumLength(50);
            RuleFor(x => x.LastName).MaximumLength(50);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone).MaximumLength(15);
            RuleFor(x => x.DriverLicenseNumber).MaximumLength(50);
        }
    }

    public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateDriverCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers.FindAsync(request.Id);
            if (driver == null)
            {
                throw new NotFoundException(nameof(Driver), request.Id);
            }

            driver.DriverLicenseNumber = request.DriverLicenseNumber;

            var updateUserResult = await _identityService.UpdateUserAsync(driver.UserId, new UpdateUserDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.Phone
            });

            if (!updateUserResult.Result.Succeeded)
            {
                throw new Exception($"Failed to update user information: {string.Join(", ", updateUserResult.Result.Errors)}");
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
