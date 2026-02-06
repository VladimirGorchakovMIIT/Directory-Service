using Directory_Service.Application.Database;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Location;
using Directory_Service.Domain.Position;
using Microsoft.EntityFrameworkCore;

namespace Directory_Service.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IReadDbContext
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Department> Departments => Set<Department>();
    
    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("ltree");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public IQueryable<Location> LocationsRead => Locations.AsNoTracking().AsQueryable();
    public IQueryable<Position> PositionsRead => Positions.AsNoTracking().AsQueryable();
    public IQueryable<Department> DepartmentRead => Departments.AsNoTracking().AsQueryable();
    public IQueryable<DepartmentLocation> DepartmentLocationsRead => DepartmentLocations.AsNoTracking().AsQueryable();
}