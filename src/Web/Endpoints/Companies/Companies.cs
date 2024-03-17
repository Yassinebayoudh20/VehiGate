using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Companies.Commands.CreateCompany;
using VehiGate.Application.Companies.Commands.DeleteCompany;
using VehiGate.Application.Companies.Commands.UpdateCompany;
using VehiGate.Application.Companies.Queries.GetCompanies;
using VehiGate.Application.Companies.Queries.GetCompanyById;

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

    private async Task<IResult> CreateCompany(ISender sender, CreateCompanyCommand command)
    {
        var result = await sender.Send(command);
        return Results.Created($"/companies/{result}", result);
    }

    private async Task<IResult> UpdateCompany(ISender sender, string id, UpdateCompanyCommand command)
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

    private async Task<IResult> DeleteCompany(ISender sender, string id)
    {
        var command = new DeleteCompanyCommand { Id = id };
        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<CompanyDto> GetCompanyById(ISender sender, string id)
    {
        var query = new GetCompanyByIdQuery { Id = id };
        return await sender.Send(query);
    }

    private async Task<PagedResult<CompanyDto>> GetCompanies(ISender sender, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int SortOrder = 1)
    {
        GetCompaniesQuery query = new GetCompaniesQuery
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
