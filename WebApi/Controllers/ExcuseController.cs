using Microsoft.AspNetCore.Mvc;
using TheGreatExcusesApplication.Application.Services;
using TheGreatExcusesApplication.Domain;

namespace TheGreatExcusesApplication.WebApi.Controllers;

[ApiController]
[Route("api/excuse")]
public class ExcuseController(IExcuseService excuseService) : ControllerBase
{
    [HttpGet("{category}")]
    public async Task<IActionResult> GetExcuse(ExcuseCategory category)
    {
        var excuse = await excuseService.FindExcuse(category);
        return Ok(excuse);
    }
    
    [HttpPost("{excuseId}/{succeeded}")]
    public async Task<IActionResult> PostExcuseScore(int excuseId, bool succeeded)
    {
        var excuse = await excuseService.RegisterExcuseScore(excuseId, succeeded);
        return Ok(excuse);
    }
}