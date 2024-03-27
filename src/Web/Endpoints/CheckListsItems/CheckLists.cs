using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.CheckLists.Queries;
using VehiGate.Application.CheckLists.Queries.GetCheckListItemsByAssociation;
using VehiGate.Application.DriverInspections.Commands.CreateDriverInspection;
using VehiGate.Domain.Enums;

namespace VehiGate.Web.Endpoints.CheckListsItems;

public class CheckLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetCheckingItemsByAssociation);
    }

    private async Task<List<CheckListItemDto>> GetCheckingItemsByAssociation(ISender sender, [FromQuery] CheckListAssociation association)
    {
        var query = new GetCheckListItemsByAssociationQuery() { Association = association };
        return await sender.Send(query);
       
    }
}
