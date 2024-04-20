using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Vehicles.Commands.CreateVehicle;
using VehiGate.Application.Vehicles.Commands.DeleteVehicle;
using VehiGate.Application.Vehicles.Commands.UpdateVehicle;
using VehiGate.Application.Vehicles.Queries.GetVehicle;
using VehiGate.Application.Vehicles.Queries.GetVehicles;

namespace VehiGate.Web.Endpoints.Vehicles
{
    public class Vehicles : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateVehicle)
                .MapPatch(UpdateVehicle, "{Id}")
                .MapDelete(DeleteVehicle, "{Id}")
                .MapGet(GetVehicleById, "{Id}")
                .MapGet(GetVehicles);
        }

        private async Task<IResult> CreateVehicle(ISender sender, CreateVehicleCommand command)
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        }

        private async Task<IResult> UpdateVehicle(ISender sender, string id, UpdateVehicleCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            var result = await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> DeleteVehicle(ISender sender, string id)
        {
            var command = new DeleteVehicleCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<VehicleDto> GetVehicleById(ISender sender, string id)
        {
            var query = new GetVehicleQuery { Id = id };
            return await sender.Send(query);
        }

        private async Task<PagedResult<VehicleDto>> GetVehicles(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int SortOrder = 1,
                                               [FromQuery] string? vehicleTypeFilter = null)
        {
            GetVehiclesQuery query = new GetVehiclesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchBy = searchBy,
                OrderBy = orderBy,
                SortOrder = SortOrder,
                VehicleTypeFilter = vehicleTypeFilter
            };

            return await sender.Send(query);
        }
    }
}
