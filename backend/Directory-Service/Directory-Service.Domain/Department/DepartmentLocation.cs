using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Location.ValueObjects;

namespace Directory_Service.Domain.Department;

public class DepartmentLocation
{
    public DepartmentLocation()
    {
    }

    public DepartmentLocation(DepartmentLocationId departmentLocationId, DepartmentId departmentId, LocationId locationId)
    {
        DepartmentLocationId = departmentLocationId;
        DepartmentId = departmentId;
        LocationId = locationId;
        CreatedAt = DateTime.UtcNow;
    }

    public DepartmentLocationId DepartmentLocationId { get; private set; }

    public DepartmentId DepartmentId { get; private set; }
    
    public LocationId LocationId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
}