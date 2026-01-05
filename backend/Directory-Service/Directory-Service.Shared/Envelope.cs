using System.Text.Json.Serialization;

namespace Directory_Service.Shared;

public record Envelope
{
    public object? Result { get; }

    public Errors.Errors ErrorsList { get; }

    public bool IsError => ErrorsList != null || (ErrorsList != null && ErrorsList.Any());

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    public Envelope(object? result, Errors.Errors errorsList)
    {
        Result = result;
        ErrorsList = errorsList;
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope Ok(object? result = null) => new Envelope(result, null);

    public static Envelope Error(Errors.Errors errorsList) => new Envelope(null, errorsList);
}

public record Envelope<T>
{
    public T? Result { get; }
    
    public Errors.Errors ErrorsList { get; }
    
    public bool IsError => ErrorsList != null || (ErrorsList != null && ErrorsList.Any());
    
    public DateTime TimeGenerated { get; }

    public Envelope(T result, Errors.Errors errorsList)
    {
        Result = result;
        ErrorsList = errorsList;
        TimeGenerated = DateTime.UtcNow;
    }
    
    public static Envelope<T> Ok(T? result = default) => new Envelope<T>(result, null);
    
    public static Envelope<T> Error(Errors.Errors errorsList) => new Envelope<T>(default, errorsList);
}