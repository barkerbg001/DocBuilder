using Microsoft.AspNetCore.Mvc;

namespace DocBuilder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeneralController : ControllerBase
{
    [HttpGet("greetings")]
    public IActionResult Greetings()
    {
        return Ok("Hello World!");
    }
}