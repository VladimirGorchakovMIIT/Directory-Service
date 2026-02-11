using Directory_Service.Domain.Position.ValueObjects;

namespace Directory_Service.Contracts.Position;

public record PositionDto
{
    public PositionId? Id { get; init; }

    public Name? Name { get; init; }

    public Description? Description { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}