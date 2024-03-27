using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Enums;

namespace VehiGate.Application.CheckLists.Queries.GetCheckListItemsByAssociation;

public class GetCheckListItemsByAssociationQuery : IRequest<List<CheckListItemDto>>
{
    public CheckListAssociation Association { get; set; }
}

public class GetCheckListItemsByAssociationQueryHandler : IRequestHandler<GetCheckListItemsByAssociationQuery, List<CheckListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCheckListItemsByAssociationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CheckListItemDto>> Handle(GetCheckListItemsByAssociationQuery request, CancellationToken cancellationToken)
    {
        var itemsByAssociation = await _context.CheckItems.Where(ci => ci.AssociatedTo == request.Association).AsNoTracking().ToListAsync(cancellationToken);

        var checkItemsDto = new List<CheckListItemDto>();

        if (itemsByAssociation.Any())
        {
            foreach (var item in itemsByAssociation)
            {
                checkItemsDto.Add(new CheckListItemDto { Id = item.Id, Name = item.Name, Association = item.AssociatedTo });
            }

            checkItemsDto = checkItemsDto.OrderBy(ci => ci.Name).ToList();

            return checkItemsDto;
        }

        return checkItemsDto;
    }
}
