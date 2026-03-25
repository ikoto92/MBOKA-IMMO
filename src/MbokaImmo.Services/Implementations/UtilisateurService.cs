using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Utilisateurs;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Helpers;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class UtilisateurService(AppDbContext context) : IUtilisateurService
{
    // ── GET ALL ──────────────────────────────────────────────────
    public async Task<PagedResultDto<UtilisateurResponseDto>> GetAllAsync(
        int page, int pageSize, string? role, string? search)
    {
        var query = context.Utilisateurs.AsQueryable();

        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role.ToString() == role);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(u =>
                u.Nom.Contains(search) ||
                u.Prenom.Contains(search) ||
                u.Email.Contains(search));

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
                DateInscription = u.DateInscription,
            })
            .ToListAsync();

        return new PagedResultDto<UtilisateurResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        };
    }

    // ── GET BY ID ────────────────────────────────────────────────
    public async Task<UtilisateurResponseDto?> GetByIdAsync(int id)
    {
        var u = await context.Utilisateurs.FirstOrDefaultAsync(u => u.IdUser == id);
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
            DateInscription = u.DateInscription,
        };
    }

    // ── UPDATE (admin) ───────────────────────────────────────────
    public async Task<UtilisateurResponseDto> UpdateAsync(int id, UtilisateurUpdateDto dto)
    {
        var u = await context.Utilisateurs.FirstOrDefaultAsync(x => x.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        if (dto.Nom is not null) u.Nom = dto.Nom;
        if (dto.Prenom is not null) u.Prenom = dto.Prenom;
        if (dto.Telephone is not null) u.Telephone = dto.Telephone;
        if (dto.PaysResidence is not null) u.PaysResidence = dto.PaysResidence;
        if (dto.VilleResidence is not null) u.VilleResidence = dto.VilleResidence;
        if (dto.PieceIdentite is not null) u.PieceIdentite = dto.PieceIdentite;

        await context.SaveChangesAsync();

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
            DateInscription = u.DateInscription,
        };
    }

    // ── DELETE ───────────────────────────────────────────────────
    public async Task DeleteAsync(int id)
    {
        var u = await context.Utilisateurs.FirstOrDefaultAsync(x => x.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        context.Utilisateurs.Remove(u);
        await context.SaveChangesAsync();
    }

    // ── TOGGLE ACTIF ─────────────────────────────────────────────
    public async Task ToggleCompteActifAsync(int id)
    {
        var u = await context.Utilisateurs.FirstOrDefaultAsync(x => x.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        u.CompteActif = !u.CompteActif;
        await context.SaveChangesAsync();
    }

    // ── VALIDER KYC ──────────────────────────────────────────────
    public async Task ValiderKycAsync(int id)
    {
        var u = await context.Utilisateurs.FirstOrDefaultAsync(x => x.IdUser == id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable.");

        u.KycValide = true;
        await context.SaveChangesAsync();
    }

    // ── GET PROFIL (connecté) ────────────────────────────────────
    public async Task<UserDto> GetProfilAsync(int idUser)
    {
        var u = await context.Utilisateurs.FindAsync(idUser)
            ?? throw new KeyNotFoundException("Utilisateur introuvable.");
        return MapToUserDto(u);
    }

    // ── UPDATE PROFIL (connecté) ─────────────────────────────────
    public async Task<UserDto> UpdateProfilAsync(int idUser, ProfilUpdateDto dto)
    {
        var u = await context.Utilisateurs.FindAsync(idUser)
            ?? throw new KeyNotFoundException("Utilisateur introuvable.");

        if (dto.Nom is not null) u.Nom = dto.Nom;
        if (dto.Prenom is not null) u.Prenom = dto.Prenom;
        if (dto.Telephone is not null) u.Telephone = dto.Telephone;
        if (dto.PaysResidence is not null) u.PaysResidence = dto.PaysResidence;
        if (dto.VilleResidence is not null) u.VilleResidence = dto.VilleResidence;

        await context.SaveChangesAsync();
        return MapToUserDto(u);
    }

    // ── CHANGE PASSWORD ──────────────────────────────────────────
    public async Task ChangePasswordAsync(int idUser, ChangePasswordDto dto)
    {
        var u = await context.Utilisateurs.FindAsync(idUser)
            ?? throw new KeyNotFoundException("Utilisateur introuvable.");

        if (!PasswordHelper.Verify(dto.AncienMotDePasse, u.MotDePasse))
            throw new UnauthorizedAccessException("Ancien mot de passe incorrect.");

        u.MotDePasse = PasswordHelper.Hash(dto.NouveauMotDePasse);
        await context.SaveChangesAsync();
    }

    // ── RESET PASSWORD REQUEST ───────────────────────────────────
    public async Task ResetPasswordRequestAsync(ResetPasswordRequestDto dto)
    {
        var u = await context.Utilisateurs
            .FirstOrDefaultAsync(x => x.Email == dto.Email && x.CompteActif);

        if (u is null) return; // Sécurité : ne pas révéler si l'email existe
        Console.WriteLine($"[TODO] Envoyer email reset password à : {u.Email}");
    }

    // ── RESET PASSWORD CONFIRM ───────────────────────────────────
    public async Task ResetPasswordConfirmAsync(ResetPasswordConfirmDto dto)
    {
        // TODO: valider le token JWT et réinitialiser le mot de passe
        await Task.CompletedTask;
    }

    // ── Mapper ───────────────────────────────────────────────────
    private static UserDto MapToUserDto(
        MBOKA_IMMO.src.MbokaImmo.Domain.Entities.Utilisateur u) => new()
        {
            IdUser = u.IdUser,
            Nom = u.Nom,
            Prenom = u.Prenom,
            Email = u.Email,
            Role = u.Role.ToString(),
            KycValide = u.KycValide,
        };
}

