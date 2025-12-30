using System.Text;
using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Location.ValueObjects;

public record Timezone(string Value)
{
    private const string SEPARATOR = "/";
    
    public static Result<Timezone, Error> Create(string continent, string city)
    {
        if(string.IsNullOrWhiteSpace(continent))
            return GeneralErrors.Validation("continent.error", "Continent is not valid");
        
        if(string.IsNullOrWhiteSpace(city))
            return GeneralErrors.Validation("city.error", "City is not valid");

        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append(continent).Append(SEPARATOR).Append(city);
        
        return new Timezone(stringBuilder.ToString());
    }
}