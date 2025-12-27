using Directory_Service.Domain.Department.ValueObjects;

namespace Directory_Service.Domain.Department;

public class DepartmentPosition
{
    public DepartmentPosition(){}

    public DepartmentPosition(DepartmentPositionId departmentPositionId, Position.Position position, Department department)
    {
        Id = departmentPositionId;
        Department = department;
        Position = position;
        CreatedAt = DateTime.UtcNow;
    }
    
    public DepartmentPositionId Id { get; private set; }
    
    public Department Department { get; private set; }
    
    public Position.Position Position { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
}