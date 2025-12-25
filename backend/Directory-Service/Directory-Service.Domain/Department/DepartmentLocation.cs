namespace Directory_Service.Domain.Department;

public class DepartmentLocation
{
    public DepartmentLocation()
    {
    }

    public DepartmentLocation(DepartmentLocationId departmentLocationId, Department department, Location.Location location)
    {
        DepartmentLocationId = departmentLocationId;
        Department = department;
        Location = location;
        CreatedAt = DateTime.UtcNow;
    }

    public DepartmentLocationId DepartmentLocationId { get; private set; }

    public Department Department { get; private set; }
    
    public Location.Location Location { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
}