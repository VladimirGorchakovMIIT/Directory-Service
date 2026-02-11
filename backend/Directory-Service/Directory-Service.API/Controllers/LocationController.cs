using Directory_Service.Application.Abstraction;
using Directory_Service.Application.Location;
using Directory_Service.Application.Location.Queries;
using Directory_Service.Contracts.Location;
using Directory_Service.Core.Endpoints;
using Microsoft.AspNetCore.Mvc;

namespace Directory_Service.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody] CreateLocationCommand command,
        [FromServices] CreateLocationHandler handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken);
    }

    [HttpGet]
    public async Task<EndpointResult<PaginationResponse<LocationDto>>> GetAllWithPaginationAndFilter(
        [FromServices] IQueriesHandler<GetLocationsCommand, PaginationResponse<LocationDto>> handler,
        [FromQuery] GetLocationsCommand command,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken);
    }
}