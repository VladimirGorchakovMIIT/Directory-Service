using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Location.ValueObjects;

public record Address(string Street, string City, int Building, int Flat)
{
    private const string Separator = "||";
    
    public static Result<Address, Error> Create(string street, string city, int building, int flat)
    {
        if(string.IsNullOrWhiteSpace(street))
            return Error.ValueIsInvalid("street.validation", "Street is not valid", "street");
        
        if(string.IsNullOrWhiteSpace(city))
            return Error.ValueIsInvalid("city.validation", "City is not valid", "city");
        
        if(building <= 0 || flat <= 0)
            return Error.ValueIsInvalid("building.or.flat.validation", " or flat is not validation", "building");
        
        return new Address(street, city, building, flat);
    }
    
    public string TranslateToString() => string.Join(Separator, Street, City, Building, Flat);
}