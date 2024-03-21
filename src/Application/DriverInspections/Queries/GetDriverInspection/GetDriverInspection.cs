using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.DriverInspections.Queries.GetDriverInspection
{
    public record GetDriverInspectionQuery : IRequest<DriverInspectionDto>
    {
        public string Id { get; init; }
    }

    public class GetDriverInspectionQueryValidator : AbstractValidator<GetDriverInspectionQuery>
    {
        public GetDriverInspectionQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetDriverInspectionQueryHandler : IRequestHandler<GetDriverInspectionQuery, DriverInspectionDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetDriverInspectionQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<DriverInspectionDto> Handle(GetDriverInspectionQuery request, CancellationToken cancellationToken)
        {
            var inspection = await _context.DriverInspections.Include(i => i.Driver).FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (inspection == null)
            {
                throw new NotFoundException(nameof(DriverInspection), request.Id);
            }

            var userById = await _identityService.GetUserById(inspection.Driver.Id);


            return new DriverInspectionDto
            {
                Id = inspection.Id,
                Driver = new DriverInformation
                {
                    Id = inspection.Driver.Id,
                    UserId = inspection.Driver.UserId,
                    Name = userById.FirstName + " " + userById.LastName,
                },
                DriversFields = inspection.DriversFields,
                AuthorizedFrom = inspection.AuthorizedFrom,
                AuthorizedTo = inspection.AuthorizedTo,
                IsAuthorized = inspection.IsAuthorized,
                Notes = inspection.Notes
            };
        }
    }
}
