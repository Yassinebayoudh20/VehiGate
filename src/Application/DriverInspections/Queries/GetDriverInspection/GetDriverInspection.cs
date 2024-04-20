using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.CheckLists.Queries;
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
            var inspection = await _context.DriverInspections.Include(i => i.Driver).Include(i => i.Checklist).ThenInclude(i => i.CheckListItems).ThenInclude(i => i.CheckItem).FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (inspection == null)
            {
                throw new NotFoundException(nameof(DriverInspection), request.Id);
            }

            var driverInspectionDto = new DriverInspectionDto
            {
                Id = inspection.Id,
                DriverId = inspection.Driver.Id,
                DriverUserId = inspection.Driver.UserId,
                Items = null,
                AuthorizedFrom = inspection.AuthorizedFrom.ToString(),
                AuthorizedTo = inspection.AuthorizedTo.ToString(),
                IsAuthorized = inspection.IsAuthorized,
                Notes = inspection.Notes
            };


            if (inspection.LastModifiedBy != null)
            {
                var lastReviewedById = await _identityService.GetUserById(inspection.LastModifiedBy);

                driverInspectionDto.ReviewedBy = lastReviewedById.FirstName + " " + lastReviewedById.LastName;


            }

            if (inspection.Driver.UserId != null)
            {
                var user = await _identityService.GetUserById(inspection.Driver.UserId);

                if (user != null)
                {
                    driverInspectionDto.DriverName = user.FirstName + " " + user.LastName;
                }
            }



            if (inspection.Checklist != null && inspection.Checklist.CheckListItems.Any())
            {
                driverInspectionDto.Items = inspection.Checklist.CheckListItems
              .Select(s => new CheckListItemDto { Id = s.CheckItemId, Name = s.CheckItem.Name, State = s.State, Note = s.Note })
              .ToList();
            }

            return driverInspectionDto;
        }
    }
}
