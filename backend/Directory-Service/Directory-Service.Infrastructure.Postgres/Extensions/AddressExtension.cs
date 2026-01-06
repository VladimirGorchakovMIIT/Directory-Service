using Directory_Service.Domain.Location;
using Directory_Service.Domain.Location.ValueObjects;

namespace Directory_Service.Infrastructure.Extensions;

public static class AddressExtension
{
    public static IQueryable<Location> WhereAddressEquals(this IQueryable<Location> query, Address address)
    {
        return query.Where( l => l.Address.Street == address.Street 
                                 && l.Address.City == address.City 
                                 && l.Address.Building == address.Building 
                                 && l.Address.Flat == address.Flat);
    } 
}