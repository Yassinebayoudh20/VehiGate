using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.DriverInspections.Commands.CreateDriverInspection;
using VehiGate.Application.DriverInspections.Commands.DeleteDriverInspection;
using VehiGate.Application.DriverInspections.Commands.UpdateDriverInspection;
using VehiGate.Application.DriverInspections.Queries.GetDriverInspection;
using VehiGate.Application.DriverInspections.Queries.GetDriverInspections;

namespace VehiGate.Web.Endpoints.VehicleInspections
{
    public class DriverInspections : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateDriverInspection)
                .MapPatch(UpdateDriverInspection, "{Id}")
                .MapDelete(DeleteDriverInspection, "{Id}")
                .MapGet(GetDriverInspectionById, "{Id}")
                .MapGet(GetDriverInspections);
        }

        private async Task<IResult> CreateDriverInspection(ISender sender, CreateDriverInspectionCommand command)
        {
            var result = await sender.Send(command);
            return Results.Created($"/driverinspections/{result}", result);
        }

        private async Task<IResult> UpdateDriverInspection(ISender sender, string id, UpdateDriverInspectionCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            var result = await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> DeleteDriverInspection(ISender sender, string id)
        {
            var command = new DeleteDriverInspectionCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<IResult> GetDriverInspectionById(ISender sender, string id)
        {
            var query = new GetDriverInspectionQuery { Id = id };
            var result = await sender.Send(query);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private async Task<IResult> GetDriverInspections(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int sortOrder = 1)
        {
            var query = new GetDriverInspectionsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchBy = searchBy,
                OrderBy = orderBy,
                SortOrder = sortOrder,
            };

            var result = await sender.Send(query);
            return Results.Ok(result);
        }
    }
}
