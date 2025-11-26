using Microsoft.AspNetCore.Mvc;
using csaspn_COLL.Services;

namespace csaspn_COLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GreetingController : ControllerBase
{
    private readonly IGreetingService _greetingService;

    public GreetingController(IGreetingService greetingService)
    {
        _greetingService = greetingService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { greeting = _greetingService.GetGreeting() });
    }
}
