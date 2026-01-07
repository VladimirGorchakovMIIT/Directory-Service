using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Position.ValueObjects;

namespace Directory_Service.Domain.Department;

public class DepartmentPosition
{
    public DepartmentPosition(){}

    public DepartmentPosition(DepartmentPositionId departmentPositionId, PositionId positionId, DepartmentId departmentId)
    {
        Id = departmentPositionId;
        DepartmentId = departmentId;
        PositionId = positionId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public DepartmentPositionId Id { get; private set; }
    
    public DepartmentId DepartmentId { get; private set; }
    
    public PositionId PositionId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
}