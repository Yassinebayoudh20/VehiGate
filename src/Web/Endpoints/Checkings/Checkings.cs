using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehiGate.Application.Checkings.Commands.CheckIn;
using VehiGate.Application.Checkings.Commands.CheckOut;
using VehiGate.Application.Checkings.Queries.GetCheckingById;
using VehiGate.Application.Checkings.Queries.GetCheckingsByStatus;
using VehiGate.Application.Companies.Queries.GetCompanies;

namespace VehiGate.Web.Endpoints.Checkings
{
    public class Checkings : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateCheckIn)
                .MapGet(GetCheckingById, "{Id}")
                .MapPatch(UpdateCheckOut, "{Id}")
                .MapGet(GetCheckingsByStatus);
        }

        private async Task<IResult> CreateCheckIn(ISender sender, CreateCheckInCommand command)
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        }

        private async Task<IResult> UpdateCheckOut(ISender sender, string id, UpdateCheckOutCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> GetCheckingById(ISender sender, string id)
        {
            var query = new GetCheckingByIdQuery { Id = id };
            var result = await sender.Send(query);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private async Task<IResult> GetCheckingsByStatus(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int SortOrder = 1)
        {

            GetCheckingsByStatusQuery query = new GetCheckingsByStatusQuery
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
