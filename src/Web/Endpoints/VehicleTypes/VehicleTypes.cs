using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Common.Models;
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

        private async Task<VehicleTypeDto> GetVehicleTypeById(ISender sender, string id)
        {
            var query = new GetVehicleTypeQuery { Id = id };
            return await sender.Send(query);
        }

        private async Task<PagedResult<VehicleTypeDto>> GetVehicleTypes(ISender sender, [FromQuery] int pageNumber = 1,
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

            return await sender.Send(query);
        }
    }
}
