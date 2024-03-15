using VehiGate.Domain.Entities;

namespace VehiGate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }

    public DbSet<Driver> Drivers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
