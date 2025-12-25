using Directory_Service.Domain.Department;

namespace Directory_Service.Domain.Location;

public class Location
{
    private readonly List<DepartmentLocation> _departmentsLocations = [];
    
    private Location()
    {
    }

    private Location(LocationId id, Name name, Address address, Timezone timezone, bool isActive, IEnumerable<DepartmentLocation> departments)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timezone;
        IsActive = isActive;
        _departmentsLocations = departments.ToList();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public LocationId Id { get; private set; }

    public Name Name { get; private set; }

    public Address Address { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<DepartmentLocation> DepartmentsLocations => _departmentsLocations;

    public static Location Create(LocationId id, Name name, Address address, Timezone timezone, bool isActive, IEnumerable<DepartmentLocation> departments) =>
        new Location(id, name, address, timezone, isActive, departments);
}