using Microsoft.AspNetCore.Mvc;
using redisAPI.Data;
using redisAPI.Models;

namespace redisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsControllers : ControllerBase
{
    private readonly IPlatformRepo _repo;

    public PlatformsControllers(IPlatformRepo repo)
    {
        _repo = repo;
        
    }

    [HttpGet("id", Name = "GetPlatformById")]
    public ActionResult<Platform> GetPlatformById(string id)
    {
        var platform = _repo.GetPlatformById(id);
        if (platform != null)
        {
            return Ok(platform);
        }
        return NotFound();
    }

    [HttpPost]
    public ActionResult<Platform> CreatePlatform(Platform platform)
    {
        _repo.CreatePlatform(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platform);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
    {
        return Ok(_repo.GetAllPlatforms());
    }
    
}