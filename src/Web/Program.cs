using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using VehiGate.Infrastructure.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(
   new ExceptionHandlerOptions()
   {
       AllowStatusCode404Response = true,
       ExceptionHandlingPath = "/error"
   });

app.MapEndpoints();

app.Run();

public partial class Program
{
}
