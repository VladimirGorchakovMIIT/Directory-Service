using Directory_Service.Application.Location;
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
        [FromBody] CreateLocationRequest request,
        [FromServices] CreateLocationHandler handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(request, cancellationToken);
    }
}