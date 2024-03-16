using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehiGate.Application.Sites.Commands.CreateSite;
using VehiGate.Application.Sites.Commands.DeleteSite;
using VehiGate.Application.Sites.Commands.UpdateSite;
using VehiGate.Application.Sites.Queries.GetSite;
using VehiGate.Application.Sites.Queries.GetSites;

namespace VehiGate.Web.Endpoints.Sites
{
    public class Sites : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateSite)
                .MapPatch(UpdateSite, "{Id}")
                .MapDelete(DeleteSite, "{Id}")
                .MapGet(GetSiteById, "{Id}")
                .MapGet(GetSites);
        }

        private async Task<IResult> CreateSite(ISender sender, CreateSiteCommand command)
        {
            var result = await sender.Send(command);
            return Results.Created($"/sites/{result}", result);
        }

        private async Task<IResult> UpdateSite(ISender sender, string id, UpdateSiteCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            var result = await sender.Send(command);

            return Results.NoContent();
        }

        private async Task<IResult> DeleteSite(ISender sender, string id)
        {
            var command = new DeleteSiteCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<IResult> GetSiteById(ISender sender, string id)
        {
            var query = new GetSiteQuery { Id = id };
            var result = await sender.Send(query);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private async Task<IResult> GetSites(ISender sender, [FromQuery] int pageNumber = 1,
                                                   [FromQuery] int pageSize = 10,
                                                   [FromQuery] string? searchBy = null,
                                                   [FromQuery] string? orderBy = null,
                                                   [FromQuery] int SortOrder = 1)
        {
            var query = new GetSitesQuery
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
