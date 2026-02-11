using System.Data;
using Directory_Service.Application.Database;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Location;
using Directory_Service.Domain.Position;
using Microsoft.EntityFrameworkCore;

namespace Directory_Service.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IReadDbContext, IDbConnectionToDatabase
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();
    
    public DbSet<DepartmentPosition> DepartmentPositions => Set<DepartmentPosition>();

    public IQueryable<Location> LocationsRead => Locations.AsNoTracking().AsQueryable();
    public IQueryable<Position> PositionsRead => Positions.AsNoTracking().AsQueryable();
    public IQueryable<Department> DepartmentRead => Departments.AsNoTracking().AsQueryable();
    public IQueryable<DepartmentLocation> DepartmentLocationsRead => DepartmentLocations.AsNoTracking().AsQueryable();

    public IQueryable<DepartmentPosition> DepartmentPositionsRead => DepartmentPositions.AsNoTracking().AsQueryable();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("ltree");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public IDbConnection GetConnection(CancellationToken cancellationToken = default) => Database.GetDbConnection();
}