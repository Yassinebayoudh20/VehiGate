using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NSwag.Generation.Processors.Security;
using NSwag;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Infrastructure.Data;
using VehiGate.Web.Services;
using VehiGate.Domain.ConfigurationOptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JwtSettingsOptions>(configuration.GetSection("JwtSettings"));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();


        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "VehiGate API";
            configure.Version = "v1";
            configure.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Bearer {your JWT token}"
            });
            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin() // need to specify from config
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
    {
        string? keyVaultUri = configuration["KeyVaultUri"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return services;
    }
}
