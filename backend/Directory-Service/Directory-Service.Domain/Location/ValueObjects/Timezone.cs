using System.Text;
using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Location.ValueObjects;

public record Timezone(string Value)
{
    private const string SEPARATOR = "/";
    
    public static Result<Timezone, Error> Create(string continent, string city)
    {
        if(string.IsNullOrWhiteSpace(continent))
            return Error.ValueIsInvalid("continent.error", "Continent is not valid", "continent");
        
        if(string.IsNullOrWhiteSpace(city))
            return Error.ValueIsInvalid("city.error", "City is not valid", "city");

        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append(continent).Append(SEPARATOR).Append(city);
        
        return new Timezone(stringBuilder.ToString());
    }
}