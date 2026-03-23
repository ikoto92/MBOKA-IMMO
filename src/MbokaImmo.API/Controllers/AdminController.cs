using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    // ── GET /api/v1/admin/statistiques ────────────────────────────
    [HttpGet("statistiques")]
    public async Task<IActionResult> GetStatistiques()
    {
        var stats = await adminService.GetStatistiquesAsync();
        return Ok(stats);
    }

    // ── GET /api/v1/admin/biens ───────────────────────────────────
    [HttpGet("biens")]
    public async Task<IActionResult> GetAllBiens()
    {
        var biens = await adminService.GetAllBiensAsync();
        return Ok(biens);
    }

    // ── GET /api/v1/admin/biens/en-attente ────────────────────────
    [HttpGet("biens/en-attente")]
    public async Task<IActionResult> GetBiensEnAttente()
    {
        var biens = await adminService.GetBiensEnAttenteAsync();
        return Ok(biens);
    }

    // ── PUT /api/v1/admin/biens/{id}/valider ──────────────────────
    [HttpPut("biens/{id:int}/valider")]
    public async Task<IActionResult> ValiderBien(int id)
    {
        try
        {
            var result = await adminService.ValiderBienAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // ── DELETE /api/v1/admin/biens/{id}/refuser ───────────────────
    [HttpDelete("biens/{id:int}/refuser")]
    public async Task<IActionResult> RefuserBien(int id)
    {
        try
        {
            await adminService.RefuserBienAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // ── GET /api/v1/admin/artisans ────────────────────────────────
    [HttpGet("artisans")]
    public async Task<IActionResult> GetAllArtisans()
    {
        var artisans = await adminService.GetAllArtisansAsync();
        return Ok(artisans);
    }

    // ── PUT /api/v1/admin/artisans/{id}/valider-kyc ───────────────
    [HttpPut("artisans/{id:int}/valider-kyc")]
    public async Task<IActionResult> ValiderKyc(int id)
    {
        try
        {
            var result = await adminService.ValiderArtisanKycAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // ── GET /api/v1/admin/interventions ───────────────────────────
    [HttpGet("interventions")]
    public async Task<IActionResult> GetAllInterventions()
    {
        var result = await adminService.GetAllInterventionsAsync();
        return Ok(result);
    }

    // ── PUT /api/v1/admin/interventions/{id}/assigner ─────────────
    [HttpPut("interventions/{id:int}/assigner")]
    public async Task<IActionResult> AssignerArtisan(
        int id, [FromBody] AssignerArtisanDto dto)
    {
        try
        {
            var result = await adminService.AssignerArtisanAsync(id, dto.IdArtisan);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // ── PUT /api/v1/admin/interventions/{id}/devis ────────────────
    [HttpPut("interventions/{id:int}/devis")]
    public async Task<IActionResult> ValiderDevis(
        int id, [FromBody] ValiderDevisDto dto)
    {
        try
        {
            var result = await adminService.ValiderDevisAsync(id, dto.Valide);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
