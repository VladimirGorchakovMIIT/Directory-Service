using CSharpFunctionalExtensions;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Shared;
using Path = Directory_Service.Domain.Department.ValueObjects.Path;

namespace Directory_Service.Domain.Department;

public sealed class Department
{
    private readonly List<Department> _childrenDepartments = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    private readonly List<DepartmentLocation> _departmentLocations = [];

    private Department()
    {
    }

    private Department(
        DepartmentId departmentDepartmentId,
        DepartmentName departmentName,
        Identifier identifier,
        Path path,
        int depth,
        IEnumerable<DepartmentLocation> departmentLocations)
    {
        DepartmentId = departmentDepartmentId;
        DepartmentName = departmentName;
        Identifier = identifier;
        Path = path;
        Depth = depth;
        _departmentLocations = departmentLocations.ToList();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public DepartmentId DepartmentId { get; private set; }

    public DepartmentName DepartmentName { get; private set; }

    public Identifier Identifier { get; private set; }

    //Возможно, нужно будет создать ValueObject, где будет производиться проверка на родителя.
    public Department? Parent { get; private set; }

    public DepartmentId? ParentId { get; private set; }

    public Path Path { get; private set; }

    public int Depth { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Department> ChildrenDepartments => _childrenDepartments;

    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;

    public static Result<Department, Error> CreateParent(
        DepartmentName departmentName,
        Identifier identifier,
        IEnumerable<DepartmentLocation> departmentLocations,
        DepartmentId? departmentId)
    {
        var departmentLocationList = departmentLocations.ToList();

        if (!departmentLocationList.Any())
            return Error.Validation("department.locations", "Departments locations must contain at least one location");

        var path = Path.CreateParent(identifier);

        return new Department(departmentId ?? new DepartmentId(Guid.NewGuid()), departmentName, identifier, path, 0, departmentLocations);
    }

    public static Result<Department, Error> CreateChild(
        DepartmentName departmentName,
        Identifier identifier,
        Department parent,
        IEnumerable<DepartmentLocation> departmentLocations,
        DepartmentId? departmentId = null)
    {
        var departmentLocationList = departmentLocations.ToList();

        if (!departmentLocationList.Any())
            return Error.Validation("department.locations", "Departments locations must contain at least one location");
        
        var path = parent.Path.CreateChild(identifier);

        return new Department(departmentId ?? new DepartmentId(Guid.NewGuid()), departmentName, identifier, path, parent.Depth + 1, departmentLocations);
    }
}