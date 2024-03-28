using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Common.Models;
using VehiGate.Application.VehicleInspections.Commands.CreateVehicleInspection;
using VehiGate.Application.VehicleInspections.Commands.DeleteVehicleInspection;
using VehiGate.Application.VehicleInspections.Commands.UpdateVehicleInspection;
using VehiGate.Application.VehicleInspections.Queries.GetVehicleInspection;
using VehiGate.Application.VehicleInspections.Queries.GetVehicleInspections;

namespace VehiGate.Web.Endpoints.VehicleInspections
{
    public class VehicleInspections : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateVehicleInspection)
                .MapPatch(UpdateVehicleInspection, "{Id}")
                .MapDelete(DeleteVehicleInspection, "{Id}")
                .MapGet(GetVehicleInspectionById, "{Id}")
                .MapGet(GetVehicleInspections);
        }

        private async Task<IResult> CreateVehicleInspection(ISender sender, CreateVehicleInspectionCommand command)
        {
            command.AuthorizedFrom = command.AuthorizedFrom.ToLocalTime();
            command.AuthorizedTo = command.AuthorizedTo.ToLocalTime();

            var result = await sender.Send(command);
            return Results.Ok(result);
        }

        private async Task<IResult> UpdateVehicleInspection(ISender sender, string id, UpdateVehicleInspectionCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            command.AuthorizedFrom = command.AuthorizedFrom.ToLocalTime();
            command.AuthorizedTo = command.AuthorizedTo.ToLocalTime();

            var result = await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> DeleteVehicleInspection(ISender sender, string id)
        {
            var command = new DeleteVehicleInspectionCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<VehicleInspectionDto> GetVehicleInspectionById(ISender sender, string id)
        {
            var query = new GetVehicleInspectionQuery { Id = id };
            return await sender.Send(query);
        }

        private async Task<PagedResult<VehicleInspectionDto>> GetVehicleInspections(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int sortOrder = 1)
        {
            var query = new GetVehicleInspectionsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchBy = searchBy,
                OrderBy = orderBy,
                SortOrder = sortOrder,
            };

            return await sender.Send(query);
        }
    }
}
