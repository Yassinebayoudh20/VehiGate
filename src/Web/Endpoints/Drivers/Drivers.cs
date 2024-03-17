﻿using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Drivers.Commands.CreateDriver;
using VehiGate.Application.Drivers.Commands.DeleteDriver;
using VehiGate.Application.Drivers.Commands.UpdateDriver;
using VehiGate.Application.Drivers.Queries.GetDriver;
using VehiGate.Application.Drivers.Queries.GetDrivers;

namespace VehiGate.Web.Endpoints.Drivers
{
    public class Drivers : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateDriver)
                .MapPatch(UpdateDriver, "{Id}")
                .MapDelete(DeleteDriver, "{Id}")
                .MapGet(GetDriverById, "{Id}")
                .MapGet(GetDrivers);
        }

        private async Task<IResult> CreateDriver(ISender sender, CreateDriverCommand command)
        {
            var result = await sender.Send(command);
            return Results.Created($"/drivers/{result}", result);
        }

        private async Task<IResult> UpdateDriver(ISender sender, string id, UpdateDriverCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> DeleteDriver(ISender sender, string id)
        {
            var command = new DeleteDriverCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<IResult> GetDriverById(ISender sender, string id)
        {
            var query = new GetDriverQuery { Id = id };
            var result = await sender.Send(query);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private async Task<IResult> GetDrivers(ISender sender, [FromQuery] int pageNumber = 1,
                                                   [FromQuery] int pageSize = 10,
                                                   [FromQuery] string? searchBy = null,
                                                   [FromQuery] string? orderBy = null,
                                                   [FromQuery] int sortOrder = 1)
        {
            var query = new GetDriversQuery
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