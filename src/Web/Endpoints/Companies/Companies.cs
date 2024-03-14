using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Companies.Commands.CreateCompany;
using VehiGate.Application.Companies.Commands.DeleteCompany;
using VehiGate.Application.Companies.Commands.UpdateCompany;
using VehiGate.Application.Companies.Queries.GetCompanies;
using VehiGate.Application.Companies.Queries.GetCompanyById;
using VehiGate.Application.Users.Queries.GetUsersList;

namespace VehiGate.Web.Endpoints.Companies;

public class Companies : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateCompany)
            .MapPatch(UpdateCompany, "{Id}")
            .MapDelete(DeleteCompany, "{Id}")
            .MapGet(GetCompanyById, "{Id}")
            .MapGet(GetCompanies);
    }

    async Task<IResult> CreateCompany(ISender sender, CreateCompanyCommand command)
    {
        var result = await sender.Send(command);
        return Results.Created($"/companies/{result}", result);
    }

    async Task<IResult> UpdateCompany(ISender sender, string id, UpdateCompanyCommand command)
    {
        if (id != command.Id)
        {
            return Results.BadRequest();
        }

        var result = await sender.Send(command);

        if (result != null)
        {
            return Results.Ok(result);
        }

        return Results.NotFound();
    }

    async Task<IResult> DeleteCompany(ISender sender, string id)
    {
        var command = new DeleteCompanyCommand { Id = id };
        await sender.Send(command);
        return Results.NoContent();
    }

    async Task<IResult> GetCompanyById(ISender sender, string id)
    {
        var query = new GetCompanyByIdQuery { Id = id };
        var result = await sender.Send(query);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    async Task<IResult> GetCompanies(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int? SortOrder = null)
    {
        GetCompaniesQuery query = new GetCompaniesQuery
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
