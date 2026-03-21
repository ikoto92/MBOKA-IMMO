using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/biens")]
public class BiensController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? ville = null,
        [FromQuery] string? type = null)
    {
        return Ok(new
        {
            items = new List<object>(),
            totalCount = 0,
            page,
            pageSize,
            totalPages = 0
        });
    }
}