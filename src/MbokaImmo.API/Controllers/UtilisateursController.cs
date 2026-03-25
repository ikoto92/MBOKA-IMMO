
using System.Security.Claims;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Utilisateurs;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/utilisateurs")]
[Produces("application/json")]
public class UtilisateursController(IUtilisateurService utilisateurService) : ControllerBase
{
    // ── Helper ───────────────────────────────────────────────────
    private int GetUserId()
        => int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("Utilisateur non authentifié.")
        );

    // ════════════════════════════════════════════════════════════
    //  ENDPOINTS ADMIN
    // ════════════════════════════════════════════════════════════

    /// <summary>Lister tous les utilisateurs (paginé)</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? role = null,
        [FromQuery] string? search = null)
    {
        var result = await utilisateurService.GetAllAsync(page, pageSize, role, search);
        return Ok(result);
    }

    /// <summary>Voir un utilisateur par ID</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await utilisateurService.GetByIdAsync(id);
        return result is null
            ? NotFound(new { error = $"Utilisateur {id} introuvable." })
            : Ok(result);
    }

    /// <summary>Modifier un utilisateur (admin)</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UtilisateurUpdateDto dto)
    {
        try
        {
            var result = await utilisateurService.UpdateAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Supprimer un utilisateur</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await utilisateurService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Activer / Désactiver un compte</summary>
    [HttpPatch("{id:int}/toggle-actif")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleActif(int id)
    {
        try
        {
            await utilisateurService.ToggleCompteActifAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Valider le KYC</summary>
    [HttpPatch("{id:int}/valider-kyc")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ValiderKyc(int id)
    {
        try
        {
            await utilisateurService.ValiderKycAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // ════════════════════════════════════════════════════════════
    //  ENDPOINTS PROFIL (utilisateur connecté)
    // ════════════════════════════════════════════════════════════

    /// <summary>Voir mon profil</summary>
    [HttpGet("profil")]
    [Authorize]
    public async Task<IActionResult> GetProfil()
    {
        try
        {
            var result = await utilisateurService.GetProfilAsync(GetUserId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Modifier mon profil</summary>
    [HttpPut("profil")]
    [Authorize]
    public async Task<IActionResult> UpdateProfil([FromBody] ProfilUpdateDto dto)
    {
        try
        {
            var result = await utilisateurService.UpdateProfilAsync(GetUserId(), dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>Changer mon mot de passe</summary>
    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            await utilisateurService.ChangePasswordAsync(GetUserId(), dto);
            return Ok(new { message = "Mot de passe modifié avec succès." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Demander réinitialisation mot de passe</summary>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordRequest(
        [FromBody] ResetPasswordRequestDto dto)
    {
        await utilisateurService.ResetPasswordRequestAsync(dto);
        return Ok(new { message = "Si cet email existe, un lien a été envoyé." });
    }

    /// <summary>Confirmer réinitialisation mot de passe</summary>
    [HttpPost("reset-password/confirm")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordConfirm(
        [FromBody] ResetPasswordConfirmDto dto)
    {
        await utilisateurService.ResetPasswordConfirmAsync(dto);
        return Ok(new { message = "Mot de passe réinitialisé avec succès." });
    }
}
