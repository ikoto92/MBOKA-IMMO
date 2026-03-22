using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;
using MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class ArtisanService(IArtisanRepository repo) : IArtisanService
{
    // ── Voir profil ──────────────────────────────────────────────
    public async Task<ArtisanResponseDto> GetProfilAsync(int idUser)
    {
        var artisan = await repo.GetByUserIdAsync(idUser)
            ?? throw new KeyNotFoundException("Artisan introuvable.");

        return MapToDto(artisan);
    }

    // ── Modifier profil ──────────────────────────────────────────
    public async Task<ArtisanResponseDto> UpdateProfilAsync(int idUser, ArtisanUpdateDto dto)
    {
        var artisan = await repo.GetByUserIdAsync(idUser)
            ?? throw new KeyNotFoundException("Artisan introuvable.");

        if (dto.Specialite is not null) artisan.Specialite = dto.Specialite;
        if (dto.ZoneIntervention is not null) artisan.ZoneIntervention = dto.ZoneIntervention;
        if (dto.Disponible is not null) artisan.Disponible = dto.Disponible.Value;
        if (dto.TarifHoraire is not null) artisan.TarifHoraire = dto.TarifHoraire;
        if (dto.Telephone is not null) artisan.Utilisateur.Telephone = dto.Telephone;

        await repo.UpdateAsync(artisan);
        await repo.SaveChangesAsync();

        return MapToDto(artisan);
    }

    // ── Recevoir mes missions ────────────────────────────────────
    public async Task<IEnumerable<MissionResponseDto>> GetMessMissionsAsync(int idArtisan)
    {
        var interventions = await repo.GetInterventionsAsync(idArtisan);
        return interventions.Select(MapToMissionDto);
    }

    // ── Soumettre un devis ───────────────────────────────────────
    public async Task<MissionResponseDto> SoumettreDevisAsync(
        int idIntervention, int idArtisan, DevisSubmitDto dto)
    {
        var intervention = await repo.GetInterventionAsync(idIntervention, idArtisan)
            ?? throw new KeyNotFoundException("Intervention introuvable.");

        if (intervention.Statut != StatutInterventionEnum.Signale)
            throw new InvalidOperationException("Un devis ne peut être soumis que pour une intervention signalée.");

        intervention.DevisMontant = dto.Montant;
        intervention.Statut = StatutInterventionEnum.Devis;
        intervention.Commentaire = dto.Description;

        await repo.UpdateInterventionAsync(intervention);
        await repo.SaveChangesAsync();

        return MapToMissionDto(intervention);
    }

    // ── Mettre à jour statut ─────────────────────────────────────
    public async Task<MissionResponseDto> UpdateStatutAsync(
        int idIntervention, int idArtisan, StatutUpdateDto dto)
    {
        var intervention = await repo.GetInterventionAsync(idIntervention, idArtisan)
            ?? throw new KeyNotFoundException("Intervention introuvable.");

        if (!Enum.TryParse<StatutInterventionEnum>(dto.Statut, true, out var statut))
            throw new ArgumentException($"Statut invalide : {dto.Statut}");

        // Vérification des transitions de statut autorisées
        var transitionsAutorisees = new Dictionary<StatutInterventionEnum, List<StatutInterventionEnum>>
        {
            { StatutInterventionEnum.Valide,  [StatutInterventionEnum.Realise] },
            { StatutInterventionEnum.Realise, [StatutInterventionEnum.Facture] },
        };

        if (transitionsAutorisees.TryGetValue(intervention.Statut, out var suivants)
            && !suivants.Contains(statut))
            throw new InvalidOperationException(
                $"Transition non autorisée : {intervention.Statut} → {statut}");

        if (statut == StatutInterventionEnum.Realise)
            intervention.DateIntervention = DateTime.UtcNow;

        intervention.Statut = statut;

        await repo.UpdateInterventionAsync(intervention);
        await repo.SaveChangesAsync();

        return MapToMissionDto(intervention);
    }

    // ── Envoyer une facture ──────────────────────────────────────
    public async Task<MissionResponseDto> EnvoyerFactureAsync(
        int idIntervention, int idArtisan, FactureSubmitDto dto)
    {
        var intervention = await repo.GetInterventionAsync(idIntervention, idArtisan)
            ?? throw new KeyNotFoundException("Intervention introuvable.");

        if (intervention.Statut != StatutInterventionEnum.Realise)
            throw new InvalidOperationException(
                "La facture ne peut être envoyée qu'après réalisation des travaux.");

        intervention.FactureUrl = dto.FactureUrl;
        intervention.Statut = StatutInterventionEnum.Facture;

        await repo.UpdateInterventionAsync(intervention);
        await repo.SaveChangesAsync();

        return MapToMissionDto(intervention);
    }

    // ── Voir mes paiements ───────────────────────────────────────
    public async Task<IEnumerable<PaiementArtisanResponseDto>> GetMesPaiementsAsync(int idArtisan)
    {
        var paiements = await repo.GetPaiementsAsync(idArtisan);
        return paiements.Select(pi => new PaiementArtisanResponseDto
        {
            IdPaiementInterv = pi.IdPaiementInterv,
            MontantArtisan = pi.MontantArtisan,
            TypeIntervention = pi.Paiement.Type.ToString(),
            DatePaiement = pi.Paiement.DatePaiement,
            Statut = pi.Paiement.Statut.ToString(),
            Reference = pi.Paiement.Reference,
        });
    }

    // ── Mappers ──────────────────────────────────────────────────
    private static ArtisanResponseDto MapToDto(
        MBOKA_IMMO.src.MbokaImmo.Domain.Entities.Artisan a) => new()
        {
            IdArtisan = a.IdArtisan,
            IdUser = a.IdUser,
            Nom = a.Utilisateur.Nom,
            Prenom = a.Utilisateur.Prenom,
            Email = a.Utilisateur.Email,
            Telephone = a.Utilisateur.Telephone,
            Specialite = a.Specialite,
            ZoneIntervention = a.ZoneIntervention,
            NoteMoyenne = a.NoteMoyenne,
            NbInterventions = a.NbInterventions,
            Disponible = a.Disponible,
            TarifHoraire = a.TarifHoraire,
        };

    private static MissionResponseDto MapToMissionDto(
        MBOKA_IMMO.src.MbokaImmo.Domain.Entities.Intervention i) => new()
        {
            IdIntervention = i.IdIntervention,
            IdLocation = i.IdLocation,
            TitreBien = i.Location?.Bien?.Titre ?? string.Empty,
            VilleBien = i.Location?.Bien?.Ville ?? string.Empty,
            AdresseBien = i.Location?.Bien?.Adresse ?? string.Empty,
            Type = i.Type,
            Description = i.Description,
            Urgence = i.Urgence.ToString(),
            Statut = i.Statut.ToString(),
            DateSignalement = i.DateSignalement,
            DateIntervention = i.DateIntervention,
            DevisMontant = i.DevisMontant,
            DevisValide = i.DevisValide,
            FactureUrl = i.FactureUrl,
            NoteLocataire = i.NoteLocataire,
        };
}

