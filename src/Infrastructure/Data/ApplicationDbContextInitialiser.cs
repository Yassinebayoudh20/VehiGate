using System.Runtime.InteropServices;
using VehiGate.Domain.Constants;
using VehiGate.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var superAdminRole = new IdentityRole(Roles.SuperAdmin);
        var administratorRole = new IdentityRole(Roles.Administrator);
        var driver = new IdentityRole(Roles.Driver);
        var user = new IdentityRole(Roles.User);

        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(superAdminRole);
            await _roleManager.CreateAsync(administratorRole);
            await _roleManager.CreateAsync(driver);
            await _roleManager.CreateAsync(user);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost", FirstName = "Admin", LastName = "Support" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        await SeedCompaniesAsync();

        await SeedDriversAsync();

        await SeedVehicleTypesAsync();

        await SeedVehiclesAsync();
    }

    private async Task SeedCompaniesAsync()
    {
        if (!_context.Companies.Any())
        {
            var company = new Company
            {
                Name = "Sample Company",
                Address = "Sample Address",
                Email = "company@example.com",
                Phone = "1234567890",
                Contact = "John Doe"
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedDriversAsync()
    {
        if (!_context.Drivers.Any())
        {
            var user = new ApplicationUser { UserName = "driver@example.com", Email = "driver@example.com", FirstName = "John", LastName = "Doe" };
            var driverRole = await _roleManager.FindByNameAsync(Roles.Driver);

            if (driverRole != null && _userManager.Users.All(u => u.UserName != user.UserName))
            {
                await _userManager.CreateAsync(user, "DriverPassword1!");
                await _userManager.AddToRoleAsync(user, driverRole.Name!);

                var company = _context.Companies.FirstOrDefault();

                if (company != null)
                {
                    var driver = new Driver
                    {
                        UserId = user.Id,
                        CompanyId = company.Id,
                        DriverLicenseNumber = "DL123456"
                    };

                    _context.Drivers.Add(driver);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

    private async Task SeedVehicleTypesAsync()
    {
        if (!_context.VehicleTypes.Any())
        {
            // Add default vehicle types
            var vehicleTypes = new[]
            {
                    new VehicleType { Name = "Car" },
                    new VehicleType { Name = "Truck" },
                };

            await _context.VehicleTypes.AddRangeAsync(vehicleTypes);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedVehiclesAsync()
    {
        if (!_context.Vehicles.Any())
        {
            var vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync();

            if (vehicleType != null)
            {
                var company = await _context.Companies.FirstOrDefaultAsync();

                if (company != null)
                {
                    // Add default vehicles
                    var vehicles = new[]
                    {
                            new Vehicle
                            {
                                VehicleType = vehicleType,
                                Company = company,
                                InsuranceCompany = "Insurance Inc.",
                                Name = "Vehicle 1",
                                PlateNumber = "ABC123",
                                InsuranceFrom = DateOnly.FromDateTime(DateTime.Now),
                                InsuranceTo = DateOnly.FromDateTime(DateTime.Today.AddDays(3))
                            },
                        };

                    await _context.Vehicles.AddRangeAsync(vehicles);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

}
