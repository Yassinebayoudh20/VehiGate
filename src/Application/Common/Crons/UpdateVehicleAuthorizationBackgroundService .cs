using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;

namespace VehiGate.Application.Common.Crons;

public class UpdateVehicleAuthorizationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UpdateVehicleAuthorizationBackgroundService> _logger;

    public UpdateVehicleAuthorizationBackgroundService(IServiceScopeFactory scopeFactory,
                                                        ILogger<UpdateVehicleAuthorizationBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background task to update vehicle authorizations.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                    var vehicles = await context.Vehicles.ToListAsync();

                    foreach (var vehicle in vehicles)
                    {
                        var isAuthorized = InspectionHelper.IsAuthorized(vehicle.AuthorizedFrom, vehicle.AuthorizedTo) && vehicle.IsAuthorized;

                        if (vehicle.IsAuthorized != isAuthorized)
                        {
                            vehicle.IsAuthorized = isAuthorized;
                            context.Vehicles.Update(vehicle);
                        }
                    }

                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation($"Updated {vehicles.Count} vehicle authorizations.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle authorizations: {@ExceptionMessage}", ex.Message);
            }
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("Background task stopped.");
    }
}
