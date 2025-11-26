using Microsoft.AspNetCore.Mvc;
using csaspn_COLL.Services;

namespace csaspn_COLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RandomController : ControllerBase
{
    private readonly IRandomNumberService _randomNumberService;

    public RandomController(IRandomNumberService randomNumberService)
    {
        _randomNumberService = randomNumberService;
    }

    [HttpGet]
    public IActionResult GetRandomNumber()
    {
        return Ok(new { number = _randomNumberService.Number });
    }
}