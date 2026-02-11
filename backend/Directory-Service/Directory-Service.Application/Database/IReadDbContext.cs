using Directory_Service.Domain.Department;
using DomainDepartment = Directory_Service.Domain.Department.Department;
using DomainPosition = Directory_Service.Domain.Position.Position;
using DomainLocation = Directory_Service.Domain.Location.Location;

namespace Directory_Service.Application.Database;

public interface IReadDbContext
{
    IQueryable<DomainLocation> LocationsRead { get; }
    IQueryable<DomainPosition> PositionsRead { get;  }
    IQueryable<DomainDepartment> DepartmentRead { get; }
    
    IQueryable<DepartmentLocation> DepartmentLocationsRead { get; }
}