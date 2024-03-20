using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.VehicleTypes.Commands.CreateVehicleType;
using VehiGate.Application.VehicleTypes.Commands.DeleteVehicleType;
using VehiGate.Application.VehicleTypes.Commands.UpdateVehicleType;
using VehiGate.Application.VehicleTypes.Queries.GetVehicleType;
using VehiGate.Application.VehicleTypes.Queries.GetVehicleTypes;

namespace VehiGate.Web.Endpoints.VehicleTypes
{
    public class VehiclesTypes : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateVehicleType)
                .MapPatch(UpdateVehicleType, "{Id}")
                .MapDelete(DeleteVehicleType, "{Id}")
                .MapGet(GetVehicleTypeById, "{Id}")
                .MapGet(GetVehicleTypes);
        }

        private async Task<IResult> CreateVehicleType(ISender sender, CreateVehicleTypeCommand command)
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        }

        private async Task<IResult> UpdateVehicleType(ISender sender, string id, UpdateVehicleTypeCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            var result = await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> DeleteVehicleType(ISender sender, string id)
        {
            var command = new DeleteVehicleTypeCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<IResult> GetVehicleTypeById(ISender sender, string id)
        {
            var query = new GetVehicleTypeQuery { Id = id };
            var result = await sender.Send(query);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private async Task<IResult> GetVehicleTypes(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int SortOrder = 1)
        {
            GetVehicleTypesQuery query = new GetVehicleTypesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchBy = searchBy,
                OrderBy = orderBy,
                SortOrder = SortOrder,
            };

            var result = await sender.Send(query);
            return Results.Ok(result);
        }
    }
}
