using CSharpFunctionalExtensions;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Department;

public class Department
{
    private readonly List<Department> _departments = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    private readonly List<DepartmentLocation> _departmentLocations = [];

    private Department()
    {
    }

    private Department(DepartmentId departmentId,
        Name name,
        Identifier identifier,
        Path path,
        Depth depth,
        Department parent,
        bool isActive,
        IEnumerable<DepartmentLocation> departmentLocations)
    {
        Id = departmentId;
        Name = name;
        Identifier = identifier;
        Path = path;
        Depth = depth;
        Parent = parent;
        IsActive = isActive;
        _departmentLocations = departmentLocations.ToList();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public DepartmentId Id { get; private set; }

    public Name Name { get; private set; }

    public Identifier Identifier { get; private set; }

    //Возможно, нужно будет создать ValueObject, где будет производиться проверка на родителя
    public Department? Parent { get; private set; }
    
    public DepartmentId? ParentId { get; private set; }

    public Path Path { get; private set; }

    public Depth Depth { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Department> DepartmentsChild => _departments;

    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;

    public static Result<Department, Error> Create(DepartmentId departmentId,
        Name name,
        Identifier identifier,
        Department parent,
        bool isActive,
        IEnumerable<DepartmentLocation> departmentLocations)
    {
        var pathResult = Path.Create(identifier.Value);
        if (pathResult.IsFailure)
            return pathResult.Error;

        var depth = Depth.Create(identifier.Value);
        if (depth.IsFailure)
            return depth.Error;

        return new Department(departmentId,
            name,
            identifier,
            pathResult.Value,
            depth.Value,
            parent,
            isActive,
            departmentLocations);
    }
}