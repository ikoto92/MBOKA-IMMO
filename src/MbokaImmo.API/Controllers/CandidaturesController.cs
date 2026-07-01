using System.Security.Claims;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Candidatures;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/candidatures")]
[Produces("application/json")]
public class CandidaturesController(ICandidatureService candidatureService) : ControllerBase
{
    private int GetLocataireId()
        => int.Parse(User.FindFirstValue("locataireId")
            ?? throw new UnauthorizedAccessException());

    private int GetProprietaireId()
        => int.Parse(User.FindFirstValue("proprietaireId")
            ?? throw new UnauthorizedAccessException());

   
    [HttpPost]
    [Authorize(Roles = "Locataire")]
    public async Task<IActionResult> Soumettre([FromBody] CandidatureCreateDto dto)
    {
        try
        {
            var result = await candidatureService
                .SoumettreCandidatureAsync(dto, GetLocataireId());
            return StatusCode(201, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    
    [HttpGet("mes-candidatures")]
    [Authorize(Roles = "Locataire")]
    public async Task<IActionResult> GetMesCandidatures()
    {
        var result = await candidatureService
            .GetMesCandidaturesAsync(GetLocataireId());
        return Ok(result);
    }

    
    [HttpGet("bien/{idBien:int}")]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> GetCandidaturesBien(int idBien)
    {
        try
        {
            var result = await candidatureService
                .GetCandidaturesBienAsync(idBien, GetProprietaireId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpPut("{id:int}/decision")]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> Decider(
        int id, [FromBody] CandidatureDecisionDto dto)
    {
        try
        {
            var result = await candidatureService
                .DeciderCandidatureAsync(id, dto, GetProprietaireId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}