using CSharpFunctionalExtensions;
using Directory_Service.Domain.Department;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Position;

public class Position
{
    private readonly List<DepartmentPosition> _positions = [];
    
    private Position()
    {
    }

    private Position(PositionId id, Name name, Description description, bool isActive, IEnumerable<DepartmentPosition> positions)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        _positions = positions.ToList();
    }

    public PositionId Id { get; private set; }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<DepartmentPosition> Positions => _positions;

    public static Position Create(PositionId id, Name name, Description description, bool isActive, IEnumerable<DepartmentPosition> positions) =>
        new Position(id, name, description, isActive, positions);
}