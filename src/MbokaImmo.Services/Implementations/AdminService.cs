using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class AdminService(AppDbContext context) : IAdminService
{
    // ── Statistiques ─────────────────────────────────────────────
    public async Task<StatistiquesDto> GetStatistiquesAsync()
    {
        var stats = new StatistiquesDto
        {
            TotalUtilisateurs = await context.Utilisateurs.CountAsync(),
            TotalProprietaires = await context.Proprietaires.CountAsync(),
            TotalLocataires = await context.Locataires.CountAsync(),
            TotalArtisans = await context.Artisans.CountAsync(),
            TotalBiens = await context.Biens.CountAsync(),
            BiensValides = await context.Biens.CountAsync(b => b.Valide),
            BiensEnAttente = await context.Biens.CountAsync(b => !b.Valide),
            BiensLoues = await context.Biens.CountAsync(b => b.Statut == StatutBienEnum.Loue),
            TotalLocations = await context.Locations.CountAsync(),
            LocationsActives = await context.Locations.CountAsync(l => l.Statut == StatutLocationEnum.Active),
            TotalPaiements = await context.Paiements.CountAsync(),
            RevenusTotaux = await context.Paiements
                .Where(p => p.Statut == StatutPaiementEnum.Reussi)
                .SumAsync(p => p.Montant),
            CommissionsTotales = await context.Paiements
                .Where(p => p.Statut == StatutPaiementEnum.Reussi)
                .SumAsync(p => p.Commission),
            TotalInterventions = await context.Interventions.CountAsync(),
            InterventionsEnCours = await context.Interventions
                .CountAsync(i => i.Statut != StatutInterventionEnum.Facture),
        };
        return stats;
    }

    // ── Biens en attente de validation ───────────────────────────
    public async Task<List<BienAdminResponseDto>> GetBiensEnAttenteAsync()
    {
        var biens = await context.Biens
            .Include(b => b.Proprietaire).ThenInclude(p => p.Utilisateur)
            .Include(b => b.Photos)
            .Where(b => !b.Valide)
            .OrderByDescending(b => b.DateCreation)
            .ToListAsync();

        return biens.Select(MapToBienAdminDto).ToList();
    }

    // ── Tous les biens ────────────────────────────────────────────
    public async Task<List<BienAdminResponseDto>> GetAllBiensAsync()
    {
        var biens = await context.Biens
            .Include(b => b.Proprietaire).ThenInclude(p => p.Utilisateur)
            .Include(b => b.Photos)
            .OrderByDescending(b => b.DateCreation)
            .ToListAsync();

        return biens.Select(MapToBienAdminDto).ToList();
    }

    // ── Valider un bien ──────────────────────────────────────────
    public async Task<BienResponseDto> ValiderBienAsync(int idBien)
    {
        var bien = await context.Biens
            .Include(b => b.Proprietaire).ThenInclude(p => p.Utilisateur)
            .Include(b => b.Photos)
            .FirstOrDefaultAsync(b => b.IdBien == idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        bien.Valide = true;
        await context.SaveChangesAsync();
        return MapToBienResponseDto(bien);
    }

    // ── Refuser un bien ──────────────────────────────────────────
    public async Task<BienResponseDto> RefuserBienAsync(int idBien)
    {
        var bien = await context.Biens
            .Include(b => b.Proprietaire).ThenInclude(p => p.Utilisateur)
            .Include(b => b.Photos)
            .FirstOrDefaultAsync(b => b.IdBien == idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        bien.Valide = false;
        context.Biens.Remove(bien);
        await context.SaveChangesAsync();
        return MapToBienResponseDto(bien);
    }

    // ── Tous les artisans ─────────────────────────────────────────
    public async Task<List<ArtisanAdminResponseDto>> GetAllArtisansAsync()
    {
        var artisans = await context.Artisans
            .Include(a => a.Utilisateur)
            .OrderBy(a => a.Utilisateur.Nom)
            .ToListAsync();

        return artisans.Select(a => new ArtisanAdminResponseDto
        {
            IdArtisan = a.IdArtisan,
            Nom = a.Utilisateur.Nom,
            Prenom = a.Utilisateur.Prenom,
            Email = a.Utilisateur.Email,
            Telephone = a.Utilisateur.Telephone,
            Specialite = a.Specialite,
            ZoneIntervention = a.ZoneIntervention,
            NoteMoyenne = a.NoteMoyenne,
            NbInterventions = a.NbInterventions,
            Disponible = a.Disponible,
            KycValide = a.Utilisateur.KycValide,
            DateInscription = a.Utilisateur.DateInscription,
        }).ToList();
    }

    // ── Valider KYC artisan ───────────────────────────────────────
    public async Task<ArtisanAdminResponseDto> ValiderArtisanKycAsync(int idArtisan)
    {
        var artisan = await context.Artisans
            .Include(a => a.Utilisateur)
            .FirstOrDefaultAsync(a => a.IdArtisan == idArtisan)
            ?? throw new KeyNotFoundException("Artisan introuvable.");

        artisan.Utilisateur.KycValide = true;
        await context.SaveChangesAsync();

        return new ArtisanAdminResponseDto
        {
            IdArtisan = artisan.IdArtisan,
            Nom = artisan.Utilisateur.Nom,
            Prenom = artisan.Utilisateur.Prenom,
            Email = artisan.Utilisateur.Email,
            Telephone = artisan.Utilisateur.Telephone,
            Specialite = artisan.Specialite,
            ZoneIntervention = artisan.ZoneIntervention,
            NoteMoyenne = artisan.NoteMoyenne,
            NbInterventions = artisan.NbInterventions,
            Disponible = artisan.Disponible,
            KycValide = artisan.Utilisateur.KycValide,
            DateInscription = artisan.Utilisateur.DateInscription,
        };
    }

    // ── Toutes les interventions ──────────────────────────────────
    public async Task<List<InterventionAdminResponseDto>> GetAllInterventionsAsync()
    {
        var interventions = await context.Interventions
            .Include(i => i.Location).ThenInclude(l => l.Bien)
            .Include(i => i.Artisan).ThenInclude(a => a.Utilisateur)
            .OrderByDescending(i => i.DateSignalement)
            .ToListAsync();

        return interventions.Select(MapToInterventionAdminDto).ToList();
    }

    // ── Assigner un artisan à une intervention ────────────────────
    public async Task<InterventionAdminResponseDto> AssignerArtisanAsync(
        int idIntervention, int idArtisan)
    {
        var intervention = await context.Interventions
            .Include(i => i.Location).ThenInclude(l => l.Bien)
            .Include(i => i.Artisan).ThenInclude(a => a.Utilisateur)
            .FirstOrDefaultAsync(i => i.IdIntervention == idIntervention)
            ?? throw new KeyNotFoundException("Intervention introuvable.");

        var artisan = await context.Artisans
            .FirstOrDefaultAsync(a => a.IdArtisan == idArtisan)
            ?? throw new KeyNotFoundException("Artisan introuvable.");

        intervention.IdArtisan = idArtisan;
        await context.SaveChangesAsync();

        // Rechargement pour la navigation
        await context.Entry(intervention)
            .Reference(i => i.Artisan).LoadAsync();
        if (intervention.Artisan is not null)
            await context.Entry(intervention.Artisan)
                .Reference(a => a.Utilisateur).LoadAsync();

        return MapToInterventionAdminDto(intervention);
    }

    // ── Valider / Refuser un devis ────────────────────────────────
    public async Task<InterventionAdminResponseDto> ValiderDevisAsync(
        int idIntervention, bool valide)
    {
        var intervention = await context.Interventions
            .Include(i => i.Location).ThenInclude(l => l.Bien)
            .Include(i => i.Artisan).ThenInclude(a => a.Utilisateur)
            .FirstOrDefaultAsync(i => i.IdIntervention == idIntervention)
            ?? throw new KeyNotFoundException("Intervention introuvable.");

        intervention.DevisValide = valide;
        if (valide)
            intervention.Statut = StatutInterventionEnum.Valide;

        await context.SaveChangesAsync();
        return MapToInterventionAdminDto(intervention);
    }

    // ── Mappers ───────────────────────────────────────────────────
    private static BienAdminResponseDto MapToBienAdminDto(
        MBOKA_IMMO.src.MbokaImmo.Domain.Entities.Bien b) => new()
        {
            IdBien = b.IdBien,
            Titre = b.Titre,
            Ville = b.Ville,
            Type = b.Type.ToString(),
            LoyerMensuel = b.LoyerMensuel,
            Statut = b.Statut.ToString(),
            Valide = b.Valide,
            NomProprietaire = b.Proprietaire?.Utilisateur != null
            ? $"{b.Proprietaire.Utilisateur.Prenom} {b.Proprietaire.Utilisateur.Nom}"
            : string.Empty,
            EmailProprietaire = b.Proprietaire?.Utilisateur?.Email ?? string.Empty,
            DateCreation = b.DateCreation,
            NbPhotos = b.Photos?.Count ?? 0,
        };

    private static BienResponseDto MapToBienResponseDto(
        MBOKA_IMMO.src.MbokaImmo.Domain.Entities.Bien b) => new()
        {
            IdBien = b.IdBien,
            IdProprietaire = b.IdProprietaire,
            NomProprietaire = b.Proprietaire?.Utilisateur != null
            ? $"{b.Proprietaire.Utilisateur.Prenom} {b.Proprietaire.Utilisateur.Nom}"
            : string.Empty,
            Titre = b.Titre,
            Description = b.Description,
            Adresse = b.Adresse,
            Ville = b.Ville,
            Quartier = b.Quartier,
            Type = b.Type.ToString(),
            Surface = b.Surface,
            Pieces = b.Pieces,
            LoyerMensuel = b.LoyerMensuel,
            CautionMois = b.CautionMois,
            Disponibilite = b.Disponibilite?.ToString("yyyy-MM-dd"),
            Statut = b.Statut.ToString(),
            Video = b.Video,
            Valide = b.Valide,
            DateCreation = b.DateCreation,
            Photos = b.Photos?.Select(p => p.Url).ToList() ?? [],
        };

    private static InterventionAdminResponseDto MapToInterventionAdminDto(
        MBOKA_IMMO.src.MbokaImmo.Domain.Entities.Intervention i) => new()
        {
            IdIntervention = i.IdIntervention,
            Type = i.Type,
            Description = i.Description,
            Urgence = i.Urgence.ToString(),
            Statut = i.Statut.ToString(),
            TitreBien = i.Location?.Bien?.Titre ?? string.Empty,
            VilleBien = i.Location?.Bien?.Ville ?? string.Empty,
            NomArtisan = i.Artisan?.Utilisateur != null
            ? $"{i.Artisan.Utilisateur.Prenom} {i.Artisan.Utilisateur.Nom}"
            : null,
            DevisMontant = i.DevisMontant,
            DevisValide = i.DevisValide,
            DateSignalement = i.DateSignalement,
            DateIntervention = i.DateIntervention,
        };
}

