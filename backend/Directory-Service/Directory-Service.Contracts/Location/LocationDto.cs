namespace Directory_Service.Contracts.Location;

public record AddressDto(string Street, string City, int Building, int Flat);

public record LocationDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = String.Empty;
    
    public AddressDto? Address { get; init; }
    
    public string Timezone { get; init; } = String.Empty;
    
    public bool IsActive { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}