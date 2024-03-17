using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehiGate.Domain.Constants;
using VehiGate.Domain.Entities;
using VehiGate.Domain.Enums;
using VehiGate.Infrastructure.Identity;

namespace VehiGate.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        ApplicationDbContextInitialiser initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

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
        IdentityRole superAdminRole = new IdentityRole(Roles.SuperAdmin);
        IdentityRole administratorRole = new IdentityRole(Roles.Administrator);
        IdentityRole driver = new IdentityRole(Roles.Driver);
        IdentityRole user = new IdentityRole(Roles.User);

        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(superAdminRole);
            await _roleManager.CreateAsync(administratorRole);
            await _roleManager.CreateAsync(driver);
            await _roleManager.CreateAsync(user);
        }

        // Default users
        ApplicationUser administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost", FirstName = "Admin", LastName = "Support" };

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

        await SeedCustomersAsync();

        await SeedSitesAsync();

        await SeedVehicleInspectionsAsync();

        await SeedDriverInspectionsAsync();

        await SeedCheckingsAsync();
    }

    private async Task SeedCompaniesAsync()
    {
        if (!_context.Companies.Any())
        {
            Company company = new Company
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
            ApplicationUser user = new ApplicationUser { UserName = "driver@example.com", Email = "driver@example.com", FirstName = "John", LastName = "Doe" };
            IdentityRole? driverRole = await _roleManager.FindByNameAsync(Roles.Driver);

            if (driverRole != null && _userManager.Users.All(u => u.UserName != user.UserName))
            {
                await _userManager.CreateAsync(user, "DriverPassword1!");
                await _userManager.AddToRoleAsync(user, driverRole.Name!);

                Company? company = _context.Companies.FirstOrDefault();

                if (company != null)
                {
                    Driver driver = new Driver
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
            VehicleType[] vehicleTypes = new[]
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
            VehicleType? vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync();

            if (vehicleType != null)
            {
                Company? company = await _context.Companies.FirstOrDefaultAsync();

                if (company != null)
                {
                    // Add default vehicles
                    Vehicle[] vehicles = new[]
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

    public async Task SeedCustomersAsync()
    {
        if (!_context.Customers.Any())
        {
            Customer[] customers = new[]
            {
                    new Customer
                    {
                        Name = "Customer 1",
                        Distance = "10 km",
                        Contact = "John Doe",
                        Phone = "123456789",
                        Email = "customer1@example.com"
                    },
                    new Customer
                    {
                        Name = "Customer 2",
                        Distance = "20 km",
                        Contact = "Jane Smith",
                        Phone = "987654321",
                        Email = "customer2@example.com"
                    },
                    // Add more customers as needed
                };

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedSitesAsync()
    {
        if (!_context.Sites.Any())
        {
            Site[] sites = new[]
            {
                    new Site
                    {
                        Address = "Site 1 Address",
                        Contact = "John Doe",
                        Phone = "123456789",
                        Email = "site1@example.com"
                    },
                    new Site
                    {
                        Address = "Site 2 Address",
                        Contact = "Jane Smith",
                        Phone = "987654321",
                        Email = "site2@example.com"
                    },
                    // Add more sites as needed
                };

            await _context.Sites.AddRangeAsync(sites);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedVehicleInspectionsAsync()
    {
        if (!_context.VehicleInspections.Any())
        {
            Driver? driver = await _context.Drivers.FirstOrDefaultAsync();
            Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync();

            if (driver != null && vehicle != null)
            {
                VehicleInspection[] inspections = new[]
                {
                new VehicleInspection
                {
                    DriverId = driver.Id,
                    VehicleId = vehicle.Id,
                    HasDocuments = true,
                    IsDamaged = false,
                    Msdn = "Sample MSDN 1",
                    AuthorizedFrom = DateTime.Now,
                    AuthorizedTo = DateTime.Now.AddDays(7),
                    Notes = "Sample notes 1"
                },
                new VehicleInspection
                {
                    DriverId = driver.Id,
                    VehicleId = vehicle.Id,
                    HasDocuments = false,
                    IsDamaged = true,
                    Msdn = "Sample MSDN 2",
                    AuthorizedFrom = DateTime.Now.AddDays(1),
                    AuthorizedTo = DateTime.Now.AddDays(8),
                    Notes = "Sample notes 2"
                },

                new VehicleInspection
                {
                    DriverId = driver.Id,
                    VehicleId = vehicle.Id,
                    HasDocuments = true,
                    IsDamaged = false,
                    Msdn = "Sample MSDN 1",
                    AuthorizedFrom = DateTime.Now.AddDays(2),
                    AuthorizedTo = DateTime.Now.AddDays(7),
                    Notes = "Sample notes 1"
                },
                new VehicleInspection
                {
                    DriverId = driver.Id,
                    VehicleId = vehicle.Id,
                    HasDocuments = false,
                    IsDamaged = true,
                    Msdn = "Sample MSDN 2",
                    AuthorizedFrom = DateTime.Now.AddDays(1),
                    AuthorizedTo = DateTime.Now.AddDays(3),
                    Notes = "Sample notes 2"
                }
            };

                await _context.VehicleInspections.AddRangeAsync(inspections);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task SeedDriverInspectionsAsync()
    {
        if (!_context.DriverInspections.Any())
        {
            Driver? driver = await _context.Drivers.FirstOrDefaultAsync();
            if (driver != null)
            {
                DriverInspection[] inspections = new[]
                {
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now,
                    AuthorizedTo = DateTime.Now.AddDays(7),
                    DriversFields = "Truck Driver",
                    Notes = "Sample notes 1"
                },
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now.AddDays(1),
                    AuthorizedTo = DateTime.Now.AddDays(8),
                    Notes = "Sample notes 2",
                    DriversFields = "Truck Driver",
                },
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now.AddDays(2),
                    AuthorizedTo = DateTime.Now.AddDays(7),
                    Notes = "Sample notes 3",
                    DriversFields = "Truck Driver",
                },
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now.AddDays(1),
                    AuthorizedTo = DateTime.Now.AddDays(3),
                    Notes = "Sample notes 4",
                    DriversFields = "Truck Driver",
                },
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now.AddDays(-2),
                    AuthorizedTo = DateTime.Now.AddDays(-1),
                    Notes = "Expired Inspection",
                    DriversFields = "Truck Driver",                },
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now.AddDays(-1),
                    AuthorizedTo = DateTime.Now.AddDays(1),
                    Notes = "Active Inspection",
                    DriversFields = "Truck Driver",                },
                new DriverInspection
                {
                    DriverId = driver.Id,
                    AuthorizedFrom = DateTime.Now.AddDays(1),
                    AuthorizedTo = DateTime.Now.AddDays(2),
                    Notes = "Future Inspection",
                    DriversFields = "Truck Driver",                }
            };

                await _context.DriverInspections.AddRangeAsync(inspections);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task SeedCheckingsAsync()
    {
        if (!_context.Checkings.Any())
        {
            var drivers = await _context.Drivers.ToListAsync();
            var vehicles = await _context.Vehicles.Include(i => i.VehicleType).ToListAsync();


            var checkings = new List<Checking>
        {
            new Checking
            {
                BLNumber = "BL123456",
                InvoiceNumber = "INV123456",
                Status = CheckingStatus.Pending,
                CheckingInDate = DateTime.Today,
                DriverId = drivers.FirstOrDefault()?.Id,
                VehicleId = vehicles.FirstOrDefault(v => v.VehicleType.Name == "Car")?.Id,
                TankId = vehicles.FirstOrDefault(v => v.VehicleType.Name == "Tank")?.Id,
            },
            new Checking
            {
                BLNumber = "BL234567",
                InvoiceNumber = "INV234567",
                Status = CheckingStatus.Completed,
                CheckingInDate = DateTime.Today.AddDays(-1),
                CheckingOutDate = DateTime.Today,
                DriverId = drivers.LastOrDefault()?.Id,
                VehicleId = vehicles.LastOrDefault(v => v.VehicleType.Name == "Car")?.Id,
                TankId = vehicles.LastOrDefault(v => v.VehicleType.Name == "Tank")?.Id,
            },
        };

            _context.Checkings.AddRange(checkings);
            await _context.SaveChangesAsync();
        }
    }
}
