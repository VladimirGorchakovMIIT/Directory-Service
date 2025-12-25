using Microsoft.AspNetCore.Mvc;

namespace Directory_Service.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetName()
    {
        return "Base Controller";
    }
}