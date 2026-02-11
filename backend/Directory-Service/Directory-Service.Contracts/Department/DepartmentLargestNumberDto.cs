namespace Directory_Service.Contracts.Department;

public record DepartmentLargestNumberDto()
{
    public int CountPositions { get; init; }
    
    public DepartmentWithOutChildrenDto? Department { get; init; }
}