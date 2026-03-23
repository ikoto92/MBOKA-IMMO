using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Candidatures;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class CandidatureService(AppDbContext context) : ICandidatureService
{
    // ── Soumettre une candidature ────────────────────────────────
    public async Task<CandidatureResponseDto> SoumettreCandidatureAsync(
        CandidatureCreateDto dto, int idLocataire)
    {
        // Vérifier que le bien existe et est disponible
        var bien = await context.Biens
            .FirstOrDefaultAsync(b => b.IdBien == dto.IdBien && b.Valide)
            ?? throw new KeyNotFoundException("Bien introuvable ou non disponible.");

        if (bien.Statut != StatutBienEnum.Libre)
            throw new InvalidOperationException("Ce bien n'est plus disponible.");

        // Vérifier qu'il n'a pas déjà candidaté
        var dejaCandidate = await context.Candidatures
            .AnyAsync(c => c.IdBien == dto.IdBien
                && c.IdLocataire == idLocataire
                && c.Statut != StatutCandidatureEnum.Refusee);

        if (dejaCandidate)
            throw new InvalidOperationException("Vous avez déjà soumis une candidature pour ce bien.");

        var candidature = new Candidature
        {
            IdBien = dto.IdBien,
            IdLocataire = idLocataire,
            MessageMotivation = dto.MessageMotivation,
            RevenusMenuels = dto.RevenusMenuels,
            PieceIdentiteUrl = dto.PieceIdentiteUrl,
            JustificatifUrl = dto.JustificatifUrl,
            Statut = StatutCandidatureEnum.EnAttente,
            DateCandidature = DateTime.UtcNow,
        };

        context.Candidatures.Add(candidature);
        await context.SaveChangesAsync();

        return await MapToDto(candidature.IdCandidature);
    }

    // ── Mes candidatures (locataire) ─────────────────────────────
    public async Task<List<CandidatureResponseDto>> GetMesCandidaturesAsync(int idLocataire)
    {
        var candidatures = await context.Candidatures
            .Include(c => c.Bien)
            .Include(c => c.Locataire).ThenInclude(l => l.Utilisateur)
            .Where(c => c.IdLocataire == idLocataire)
            .OrderByDescending(c => c.DateCandidature)
            .ToListAsync();

        return candidatures.Select(MapToCandidatureDto).ToList();
    }

    // ── Candidatures d'un bien (propriétaire) ────────────────────
    public async Task<List<CandidatureResponseDto>> GetCandidaturesBienAsync(
        int idBien, int idProprietaire)
    {
        var bien = await context.Biens
            .FirstOrDefaultAsync(b => b.IdBien == idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        if (bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Accès non autorisé.");

        var candidatures = await context.Candidatures
            .Include(c => c.Bien)
            .Include(c => c.Locataire).ThenInclude(l => l.Utilisateur)
            .Where(c => c.IdBien == idBien)
            .OrderByDescending(c => c.DateCandidature)
            .ToListAsync();

        return candidatures.Select(MapToCandidatureDto).ToList();
    }

    // ── Accepter / Refuser une candidature ───────────────────────
    public async Task<CandidatureResponseDto> DeciderCandidatureAsync(
        int idCandidature, CandidatureDecisionDto dto, int idProprietaire)
    {
        var candidature = await context.Candidatures
            .Include(c => c.Bien)
            .Include(c => c.Locataire).ThenInclude(l => l.Utilisateur)
            .FirstOrDefaultAsync(c => c.IdCandidature == idCandidature)
            ?? throw new KeyNotFoundException("Candidature introuvable.");

        if (candidature.Bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Accès non autorisé.");

        if (candidature.Statut != StatutCandidatureEnum.EnAttente)
            throw new InvalidOperationException("Cette candidature a déjà été traitée.");

        if (dto.Acceptee)
        {
            candidature.Statut = StatutCandidatureEnum.Acceptee;

            // Marquer le bien comme EnCours
            candidature.Bien.Statut = StatutBienEnum.EnCours;

            // Refuser toutes les autres candidatures pour ce bien
            var autresCandidatures = await context.Candidatures
                .Where(c => c.IdBien == candidature.IdBien
                    && c.IdCandidature != idCandidature
                    && c.Statut == StatutCandidatureEnum.EnAttente)
                .ToListAsync();

            foreach (var autre in autresCandidatures)
                autre.Statut = StatutCandidatureEnum.Refusee;
        }
        else
        {
            candidature.Statut = StatutCandidatureEnum.Refusee;
        }

        await context.SaveChangesAsync();
        return MapToCandidatureDto(candidature);
    }

    // ── Mapper ───────────────────────────────────────────────────
    private async Task<CandidatureResponseDto> MapToDto(int idCandidature)
    {
        var c = await context.Candidatures
            .Include(c => c.Bien)
            .Include(c => c.Locataire).ThenInclude(l => l.Utilisateur)
            .FirstAsync(c => c.IdCandidature == idCandidature);
        return MapToCandidatureDto(c);
    }

    private static CandidatureResponseDto MapToCandidatureDto(Candidature c) => new()
    {
        IdCandidature = c.IdCandidature,
        IdBien = c.IdBien,
        TitreBien = c.Bien?.Titre ?? string.Empty,
        VilleBien = c.Bien?.Ville ?? string.Empty,
        LoyerMensuel = c.Bien?.LoyerMensuel ?? 0,
        IdLocataire = c.IdLocataire,
        NomLocataire = c.Locataire?.Utilisateur?.Nom ?? string.Empty,
        PrenomLocataire = c.Locataire?.Utilisateur?.Prenom ?? string.Empty,
        EmailLocataire = c.Locataire?.Utilisateur?.Email ?? string.Empty,
        TelephoneLocataire = c.Locataire?.Utilisateur?.Telephone,
        MessageMotivation = c.MessageMotivation,
        RevenusMenuels = c.RevenusMenuels,
        PieceIdentiteUrl = c.PieceIdentiteUrl,
        JustificatifUrl = c.JustificatifUrl,
        Statut = c.Statut.ToString(),
        DateCandidature = c.DateCandidature,
    };
}
