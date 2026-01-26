using Microsoft.AspNetCore.Mvc;
using BlackDragonAPI.Models;

namespace BlackDragonAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EchoController : ControllerBase
{
    [HttpPost]
    public IActionResult Echo(EchoRequest request)
    {
        return Ok(request);
    }
}
