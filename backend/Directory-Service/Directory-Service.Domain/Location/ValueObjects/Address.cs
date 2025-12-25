using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Location;

public record Address(string Street, string City, int Building, int Flat)
{
    public static Result<Address, Error> Create(string street, string city, int building, int flat)
    {
        if(string.IsNullOrWhiteSpace(street))
            return Error.Validation("street.validation", "Street is not valid");
        
        if(string.IsNullOrWhiteSpace(city))
            return Error.Validation("city.validation", "City is not valid");
        
        if(building <= 0 || flat <= 0)
            return Error.Validation("building.or.flat.validation", " or flat is not validation");
        
        return new Address(street, city, building, flat);
    }
}