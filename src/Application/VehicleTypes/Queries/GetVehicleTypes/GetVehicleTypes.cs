using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Application.VehicleTypes.Queries.GetVehicleType;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.VehicleTypes.Queries.GetVehicleTypes
{
    [Authorize]
    public record GetVehicleTypesQuery : IRequest<PagedResult<VehicleTypeDto>>
    {
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetVehicleTypesQueryValidator : AbstractValidator<GetVehicleTypesQuery>
    {
        public GetVehicleTypesQueryValidator()
        {
            // Add validation rules if needed
        }
    }

    public class GetVehicleTypesQueryHandler : IRequestHandler<GetVehicleTypesQuery, PagedResult<VehicleTypeDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetVehicleTypesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<VehicleTypeDto>> Handle(GetVehicleTypesQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.VehicleTypes.ToListAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                query = query.Where(vehicleType =>
                    vehicleType.Name.Contains(request.SearchBy)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;
                query = query.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            var totalCount = query.Count();


            var vehicleTypeDtos = query.Select(vt => new VehicleTypeDto
            {
                Id = vt.Id,
                Name = vt.Name
            }).ToList();

            return PagedResult<VehicleTypeDto>.Create(vehicleTypeDtos, request.PageNumber, request.PageSize);
        }
    }
}
