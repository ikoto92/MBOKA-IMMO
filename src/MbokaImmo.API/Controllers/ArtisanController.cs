using System.Security.Claims;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/artisans")]
[Authorize(Roles = "Artisan")]
[Produces("application/json")]
public class ArtisanController(IArtisanService artisanService) : ControllerBase
{
   
    private int GetUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Utilisateur non authentifié."));

    private int GetArtisanId()
        => int.Parse(User.FindFirstValue("artisanId")
            ?? throw new UnauthorizedAccessException("Artisan non authentifié."));

    
    [HttpGet("profil")]
    [ProducesResponseType(typeof(ArtisanResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfil()
    {
        try
        {
            var result = await artisanService.GetProfilAsync(GetUserId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    
    [HttpPut("profil")]
    [ProducesResponseType(typeof(ArtisanResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfil([FromBody] ArtisanUpdateDto dto)
    {
        try
        {
            var result = await artisanService.UpdateProfilAsync(GetUserId(), dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

   
    [HttpGet("missions")]
    [ProducesResponseType(typeof(IEnumerable<MissionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMissions()
    {
        try
        {
            var result = await artisanService.GetMessMissionsAsync(GetArtisanId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    
    [HttpPost("missions/{idIntervention:int}/devis")]
    [ProducesResponseType(typeof(MissionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoumettreDevis(
        int idIntervention,
        [FromBody] DevisSubmitDto dto)
    {
        try
        {
            var result = await artisanService.SoumettreDevisAsync(
                idIntervention, GetArtisanId(), dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    
    [HttpPut("missions/{idIntervention:int}/statut")]
    [ProducesResponseType(typeof(MissionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatut(
        int idIntervention,
        [FromBody] StatutUpdateDto dto)
    {
        try
        {
            var result = await artisanService.UpdateStatutAsync(
                idIntervention, GetArtisanId(), dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

   
    [HttpPost("missions/{idIntervention:int}/facture")]
    [ProducesResponseType(typeof(MissionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EnvoyerFacture(
        int idIntervention,
        [FromBody] FactureSubmitDto dto)
    {
        try
        {
            var result = await artisanService.EnvoyerFactureAsync(
                idIntervention, GetArtisanId(), dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    
    [HttpGet("paiements")]
    [ProducesResponseType(typeof(IEnumerable<PaiementArtisanResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaiements()
    {
        try
        {
            var result = await artisanService.GetMesPaiementsAsync(GetArtisanId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}

