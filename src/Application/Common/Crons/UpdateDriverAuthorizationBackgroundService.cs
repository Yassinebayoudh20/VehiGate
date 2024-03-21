using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;

namespace VehiGate.Application.Common.Crons;
public class UpdateDriverAuthorizationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UpdateDriverAuthorizationBackgroundService> _logger;

    public UpdateDriverAuthorizationBackgroundService(IServiceScopeFactory scopeFactory,
                                                       ILogger<UpdateDriverAuthorizationBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background task to update driver authorizations.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                    var drivers = await context.Drivers.ToListAsync();

                    foreach (var driver in drivers)
                    {
                        var isAuthorized = InspectionHelper.IsAuthorized(driver.AuthorizedFrom, driver.AuthorizedTo) && driver.IsAuthorized;

                        if (driver.IsAuthorized != isAuthorized)
                        {
                            driver.IsAuthorized = isAuthorized;
                            context.Drivers.Update(driver);
                        }
                    }

                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation($"Updated {drivers.Count} driver authorizations.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating driver authorizations: {@ExceptionMessage}", ex.Message);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("Background task stopped.");
    }
}
