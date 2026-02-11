namespace Directory_Service.Contracts.Location;

public record GetLocationsDto
{
    public int TotalCount { get; init; }
    
    public List<LocationDto> Locations { get; init; } = [];
};