
using System.Security.Claims;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Virements;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/virements")]
[Authorize(Roles = "Proprio")]
[Produces("application/json")]
public class VirementsController(IVirementService virementService) : ControllerBase
{
    private int GetProprietaireId()
        => int.Parse(
            User.FindFirst("proprietaireId")?.Value
            ?? throw new UnauthorizedAccessException("Propriétaire non authentifié.")
        );

    /// <summary>Demander un virement</summary>
    [HttpPost]
    public async Task<IActionResult> DemanderVirement([FromBody] VirementCreateDto dto)
    {
        try
        {
            var result = await virementService
                .DemanderVirementAsync(dto, GetProprietaireId());
            return StatusCode(201, result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    /// <summary>Voir mes virements</summary>
    [HttpGet]
    public async Task<IActionResult> GetMesVirements()
    {
        try
        {
            var result = await virementService
                .GetMesVirementsAsync(GetProprietaireId());
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}
