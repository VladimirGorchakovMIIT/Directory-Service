using Directory_Service.Application.Department;
using Directory_Service.Application.Department.Queries;
using Directory_Service.Contracts.Department;
using Directory_Service.Core.Endpoints;
using Microsoft.AspNetCore.Mvc;

namespace Directory_Service.Core.Controllers;

public record LocationsIdsRequest( IEnumerable<Guid> LocationsIds);

[ApiController]
[Route("/api/[controller]")]
public class DepartmentsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody] CreateDepartmentCommand command,
        [FromServices] CreateDepartmentHandler handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken);
    }
    
    [HttpPut("/{departmentId:guid}/locations")]
    public async Task<EndpointResult<Guid>> UpdateLocations(
        [FromRoute] Guid departmentId,
        [FromServices] UpdateLocationsDepartmentHandler handler,
        [FromBody]LocationsIdsRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationsDepartmentCommand(departmentId, request.LocationsIds);
        return await handler.Handle(command, cancellationToken);
    }

    [HttpPut("{departmentId:guid}/parent")]
    public async Task<EndpointResult<Guid>> Change(
        [FromServices] MoveDepartmentHandler handler,
        [FromRoute] Guid departmentId,
        [FromQuery] Guid parentId,
        CancellationToken cancellationToken)
    {
        var command = new ChangeDepartmentCommand(parentId, departmentId);
        return await handler.Handle(command, cancellationToken);
    }
    
    [HttpGet("/top-positions")]
    public async Task<EndpointResult<IEnumerable<DepartmentLargestNumberDto>>> GetLargestNumberPosition(
        [FromQuery] GetLargestNumberCommand command,
        [FromServices] GetLargestNumberHandler handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken);
    }
    
}