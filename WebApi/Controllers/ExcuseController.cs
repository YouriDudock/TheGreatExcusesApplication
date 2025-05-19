using Microsoft.AspNetCore.Mvc;
using TheGreatExcusesApplication.Application.Services;
using TheGreatExcusesApplication.Domain;

namespace TheGreatExcusesApplication.WebApi.Controllers;

[ApiController]
[Route("api/excuses")]
public class ExcuseController(IExcuseService excuseService) : ControllerBase
{
    [HttpGet("{category}")]
    public async Task<IActionResult> GetExcuse(ExcuseCategory category)
    {
        var excuse = await excuseService.FindExcuse(category);
        return Ok(excuse);
    }
}