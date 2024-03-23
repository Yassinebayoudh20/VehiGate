using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Drivers.Queries.GetDriver
{
    [Authorize]
    public record GetDriverQuery : IRequest<DriverDto>
    {
        public string Id { get; init; }
    }

    public class GetDriverQueryValidator : AbstractValidator<GetDriverQuery>
    {
        public GetDriverQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class GetDriverQueryHandler : IRequestHandler<GetDriverQuery, DriverDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetDriverQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<DriverDto> Handle(GetDriverQuery request, CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers.Include(i => i.Company).FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (driver == null)
            {
                throw new NotFoundException(nameof(Driver), request.Id);
            }

            UserModel userInfo = await _identityService.GetUserById(driver.UserId);

            if (userInfo == null)
            {
                throw new NotFoundException(nameof(UserModel), driver.UserId);
            }

            return new DriverDto
            {
                Id = driver.UserId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                Phone = userInfo.PhoneNumber,
                CompanyName = driver.Company.Name,
                CompanyId = driver.Company.Id,
                DriverLicenseNumber = driver.DriverLicenseNumber
            };
        }
    }
}
