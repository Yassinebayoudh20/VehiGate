using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehiGate.Application.Customers.Commands.CreateCustomer;
using VehiGate.Application.Customers.Commands.DeleteCustomer;
using VehiGate.Application.Customers.Commands.UpdateCustomer;
using VehiGate.Application.Customers.Queries.GetCustomer;
using VehiGate.Application.Customers.Queries.GetCustomers;

namespace VehiGate.Web.Endpoints.Customers
{
    public class Customers : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateCustomer)
                .MapPatch(UpdateCustomer, "{Id}")
                .MapDelete(DeleteCustomer, "{Id}")
                .MapGet(GetCustomer, "{Id}")
                .MapGet(GetCustomers);
        }

        private async Task<IResult> CreateCustomer(ISender sender, CreateCustomerCommand command)
        {
            var result = await sender.Send(command);
            return Results.Created($"/customers/{result}", result);
        }

        private async Task<IResult> UpdateCustomer(ISender sender, string id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            var result = await sender.Send(command);

            return Results.NoContent();

        }

        private async Task<IResult> DeleteCustomer(ISender sender, string id)
        {
            var command = new DeleteCustomerCommand { Id = id };
            await sender.Send(command);
            return Results.NoContent();
        }

        private async Task<IResult> GetCustomer(ISender sender, string id)
        {
            var query = new GetCustomerQuery { Id = id };
            var result = await sender.Send(query);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private async Task<IResult> GetCustomers(ISender sender, [FromQuery] int pageNumber = 1,
                                                   [FromQuery] int pageSize = 10,
                                                   [FromQuery] string? searchBy = null,
                                                   [FromQuery] string? orderBy = null,
                                                   [FromQuery] int SortOrder = 1)
        {
            var query = new GetCustomersQuery
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
