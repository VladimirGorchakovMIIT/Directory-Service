namespace Directory_Service.Contracts.Department;

public sealed class DepartmentDto
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string Identifier { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Depth { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<DepartmentDto> Children { get; set; } = [];
}