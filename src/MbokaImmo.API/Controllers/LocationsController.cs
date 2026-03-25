using System.Security.Claims;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Locations;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/locations")]
[Produces("application/json")]
public class LocationsController(ILocationService locationService) : ControllerBase
{
    private int GetProprietaireId()
        => int.Parse(User.FindFirstValue("proprietaireId")
            ?? throw new UnauthorizedAccessException());

    private int GetLocataireId()
        => int.Parse(User.FindFirstValue("locataireId")
            ?? throw new UnauthorizedAccessException());

    [HttpPost]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> CreerBail([FromBody] LocationCreateDto dto)
    {
        try
        {
            var result = await locationService.CreerBailAsync(dto, GetProprietaireId());
            return StatusCode(201, result);
        }
        catch (KeyNotFoundException ex)      { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException)  { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("mes-locations")]
    [Authorize(Roles = "Locataire")]
    public async Task<IActionResult> GetMesLocationsLocataire()
    {
        var result = await locationService
            .GetMesLocationsLocataireAsync(GetLocataireId());
        return Ok(result);
    }

    [HttpGet("mes-biens-loues")]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> GetMesLocationsProprietaire()
    {
        var result = await locationService
            .GetMesLocationsProprietaireAsync(GetProprietaireId());
        return Ok(result);
    }

    [HttpPut("{id:int}/terminer")]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> TerminerLocation(int id)
    {
        try
        {
            var result = await locationService
                .TerminerLocationAsync(id, GetProprietaireId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex)     { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }
}
