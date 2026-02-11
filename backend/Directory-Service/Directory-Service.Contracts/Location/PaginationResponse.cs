namespace Directory_Service.Contracts.Location;

public record PaginationResponse<T>
{
    public int TotalCount { get; init; }
    
    public int Page { get; init; }
    
    public int PageSize { get; init; }
    
    public List<T> Items { get; init; } = [];
};