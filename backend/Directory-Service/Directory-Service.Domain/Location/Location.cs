using CSharpFunctionalExtensions;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Location;

public class Location
{
    private readonly List<DepartmentLocation> _departmentsLocations = [];
    
    private Location(){}
    
    public Location(LocationId id, string name, Address address, Timezone timezone, bool isActive, IEnumerable<DepartmentLocation> departments)
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

    public string Name { get; private set; }

    public Address Address { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<DepartmentLocation> DepartmentsLocations => _departmentsLocations;
}