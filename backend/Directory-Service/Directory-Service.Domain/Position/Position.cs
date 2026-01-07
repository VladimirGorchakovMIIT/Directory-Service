using CSharpFunctionalExtensions;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Position.ValueObjects;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Position;

public class Position
{
    private readonly List<DepartmentPosition> _departmentPosition = [];
    
    private Position() { }

    private Position(PositionId id, Name name, Description description, IEnumerable<DepartmentPosition> positions)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        _departmentPosition = positions.ToList();
    }

    public PositionId Id { get; private set; }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<DepartmentPosition> DepartmentPosition => _departmentPosition;

    public static Position Create(PositionId id, Name name, Description description, IEnumerable<DepartmentPosition> positions) =>
        new Position(id, name, description, positions);

    public static IEnumerable<DepartmentPosition> LinkDepartmentPosition(IEnumerable<Guid> departmentsIds, Guid positionId)
    {
        List<DepartmentPosition> departmentPositions = [];

        foreach (var departmentsId in departmentsIds)
        {
            var departmentPosition = new DepartmentPosition(new DepartmentPositionId(Guid.NewGuid()), 
                new PositionId(positionId), 
                new DepartmentId(departmentsId));
            
            departmentPositions.Add(departmentPosition);
        }
        
        return departmentPositions;
    }
}