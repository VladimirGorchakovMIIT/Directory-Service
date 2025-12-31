using Directory_Service.Domain.Location;
using Directory_Service.Domain.Location.ValueObjects;

namespace Directory_Service.Contracts.Location;

public record CreateLocationRequest(string Name, AddressRequest Address, TimezoneRequest Timezone);