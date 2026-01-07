using Directory_Service.Application.Location;
using Directory_Service.Application.Position;
using Directory_Service.Core.Endpoints;
using Microsoft.AspNetCore.Mvc;

namespace Directory_Service.Core.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PositionController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody] PositionCreateCommand command, 
        [FromServices] CreatePositionHandler handler, 
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken);
    }
}