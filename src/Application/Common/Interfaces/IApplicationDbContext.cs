using VehiGate.Domain.Entities;

namespace VehiGate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }

    DbSet<Driver> Drivers { get; }

    DbSet<Vehicle> Vehicles { get; }

    DbSet<VehicleType> VehicleTypes { get; }

    DbSet<Site> Sites { get; }

    DbSet<Customer> Customers { get; }

    DbSet<VehicleInspection> VehicleInspections { get; }

    DbSet<DriverInspection> DriverInspections { get; }

    DbSet<Checking> Checkings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
