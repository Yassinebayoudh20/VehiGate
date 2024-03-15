using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleTypes.Queries.GetVehicleType
{
    [Authorize]
    public record GetVehicleTypeQuery : IRequest<VehicleTypeDto>
    {
        public string Id { get; init; }
    }

    public class GetVehicleTypeQueryValidator : AbstractValidator<GetVehicleTypeQuery>
    {
        public GetVehicleTypeQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class GetVehicleTypeQueryHandler : IRequestHandler<GetVehicleTypeQuery, VehicleTypeDto>
    {
        private readonly IApplicationDbContext _context;

        public GetVehicleTypeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleTypeDto> Handle(GetVehicleTypeQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.VehicleTypes.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(VehicleType), request.Id);
            }

            return new VehicleTypeDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
