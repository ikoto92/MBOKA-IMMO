using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Utilisateurs;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class UtilisateurService : IUtilisateurService
{
    private readonly AppDbContext _context;

    public UtilisateurService(AppDbContext context)
    {
        _context = context;
    }

    // ── GET ALL (avec pagination, filtre rôle, recherche) ────────
    public async Task<PagedResultDto<UtilisateurResponseDto>> GetAllAsync(
        int page, int pageSize, string? role, string? search)
    {
        var query = _context.Utilisateurs.AsQueryable();

        // Filtre par rôle
        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role.ToString() == role);

        // Recherche par nom, prénom ou email
        if (!string.IsNullOrEmpty(search))
            query = query.Where(u =>
                u.Nom.Contains(search) ||
                u.Prenom.Contains(search) ||
                u.Email.Contains(search)
            );

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(u => u.DateInscription)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UtilisateurResponseDto
            {
                IdUser = u.IdUser,
                Nom = u.Nom,
                Prenom = u.Prenom,
                Email = u.Email,
                Telephone = u.Telephone,
                PaysResidence = u.PaysResidence,
                VilleResidence = u.VilleResidence,
                Role = u.Role.ToString(),
                KycValide = u.KycValide,
                CompteActif = u.CompteActif,
                DateInscription = u.DateInscription
            })
            .ToListAsync();

        return new PagedResultDto<UtilisateurResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    // ── GET BY ID ────────────────────────────────────────────────
    public async Task<UtilisateurResponseDto?> GetByIdAsync(int id)
    {
        var u = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.IdUser == id);

        if (u is null) return null;

        return new UtilisateurResponseDto
        {
            IdUser = u.IdUser,
            Nom = u.Nom,
            Prenom = u.Prenom,
            Email = u.Email,
            Telephone = u.Telephone,
            PaysResidence = u.PaysResidence,
            VilleResidence = u.VilleResidence,
            Role = u.Role.ToString(),
            KycValide = u.KycValide,
            CompteActif = u.CompteActif,
            DateInscription = u.DateInscription
        };
    }

    // ── UPDATE ───────────────────────────────────────────────────
    public async Task<UtilisateurResponseDto> UpdateAsync(int id, UtilisateurUpdateDto dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        // Mise à jour uniquement des champs fournis
        if (dto.Nom != null) utilisateur.Nom = dto.Nom;
        if (dto.Prenom != null) utilisateur.Prenom = dto.Prenom;
        if (dto.Telephone != null) utilisateur.Telephone = dto.Telephone;
        if (dto.PaysResidence != null) utilisateur.PaysResidence = dto.PaysResidence;
        if (dto.VilleResidence != null) utilisateur.VilleResidence = dto.VilleResidence;
        if (dto.PieceIdentite != null) utilisateur.PieceIdentite = dto.PieceIdentite;

        await _context.SaveChangesAsync();

        return new UtilisateurResponseDto
        {
            IdUser = utilisateur.IdUser,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            Telephone = utilisateur.Telephone,
            PaysResidence = utilisateur.PaysResidence,
            VilleResidence = utilisateur.VilleResidence,
            Role = utilisateur.Role.ToString(),
            KycValide = utilisateur.KycValide,
            CompteActif = utilisateur.CompteActif,
            DateInscription = utilisateur.DateInscription
        };
    }

    // ── DELETE ───────────────────────────────────────────────────
    public async Task DeleteAsync(int id)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        _context.Utilisateurs.Remove(utilisateur);
        await _context.SaveChangesAsync();
    }

    // ── TOGGLE COMPTE ACTIF (activer/désactiver) ─────────────────
    public async Task ToggleCompteActifAsync(int id)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        utilisateur.CompteActif = !utilisateur.CompteActif;
        await _context.SaveChangesAsync();
    }

    // ── VALIDER KYC ──────────────────────────────────────────────
    public async Task ValiderKycAsync(int id)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        utilisateur.KycValide = true;
        await _context.SaveChangesAsync();
    }
}