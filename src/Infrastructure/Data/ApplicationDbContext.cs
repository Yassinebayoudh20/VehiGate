using System.Reflection;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Driver> Drivers => Set<Driver>();

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();

    public DbSet<Site> Sites => Set<Site>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<VehicleInspection> VehicleInspections => Set<VehicleInspection>();

    public DbSet<DriverInspection> DriverInspections => Set<DriverInspection>();

    public DbSet<Checking> Checkings => Set<Checking>();

    public DbSet<DriverInspectionChecklist> DriverInspectionChecklists => Set<DriverInspectionChecklist>();

    public DbSet<VehicleInspectionChecklist> VehicleInspectionChecklists => Set<VehicleInspectionChecklist>();

    public DbSet<Checklist> Checklists => Set<Checklist>();

    public DbSet<CheckItem> CheckItems => Set<CheckItem>();

    public DbSet<CheckListItem> CheckListItems => Set<CheckListItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
