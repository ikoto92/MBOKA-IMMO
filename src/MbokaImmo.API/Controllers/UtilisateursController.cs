using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Utilisateurs;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/utilisateurs")]
[Produces("application/json")]
public class UtilisateursController : ControllerBase
{
    private readonly IUtilisateurService _service;

    public UtilisateursController(IUtilisateurService service)
    {
        _service = service;
    }

    /// <summary>Récupérer la liste des utilisateurs (paginée)</summary>
    /// <param name="page">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de la page (défaut: 10)</param>
    /// <param name="role">Filtrer par rôle (Proprio, Locataire, Agent, Artisan, Admin)</param>
    /// <param name="search">Rechercher par nom, prénom ou email</param>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<UtilisateurResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? role = null,
        [FromQuery] string? search = null)
    {
        var result = await _service.GetAllAsync(page, pageSize, role, search);
        return Ok(result);
    }

    /// <summary>Récupérer un utilisateur par son ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UtilisateurResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var utilisateur = await _service.GetByIdAsync(id);
        return utilisateur is null
            ? NotFound(new { error = $"Utilisateur {id} introuvable." })
            : Ok(utilisateur);
    }

    /// <summary>Modifier un utilisateur</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UtilisateurResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UtilisateurUpdateDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Supprimer un utilisateur</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Activer ou désactiver un compte utilisateur</summary>
    [HttpPatch("{id}/toggle-actif")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActif(int id)
    {
        try
        {
            await _service.ToggleCompteActifAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Valider le KYC d'un utilisateur</summary>
    [HttpPatch("{id}/valider-kyc")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValiderKyc(int id)
    {
        try
        {
            await _service.ValiderKycAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
