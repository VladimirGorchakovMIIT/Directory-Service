using Directory_Service.Application.Location;
using Directory_Service.Contracts.Location;
using Microsoft.AspNetCore.Mvc;

namespace Directory_Service.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateLocationRequest request,
        [FromServices] CreateLocationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return result.Value;
    }
}