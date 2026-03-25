using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Locations;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class LocationService(AppDbContext context) : ILocationService
{
    public async Task<LocationResponseDto> CreerBailAsync(
        LocationCreateDto dto, int idProprietaire)
    {
        var bien = await context.Biens
            .Include(b => b.Proprietaire)
            .FirstOrDefaultAsync(b => b.IdBien == dto.IdBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        if (bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Accès non autorisé.");

        if (bien.Statut != StatutBienEnum.Libre && bien.Statut != StatutBienEnum.EnCours)
            throw new InvalidOperationException("Ce bien n'est pas disponible.");

        var location = new Location
        {
            IdBien = dto.IdBien,
            IdLocataire = dto.IdLocataire,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin,
            Caution = dto.Caution,
            CautionPayee = dto.CautionPayee,
            BailUrl = dto.BailUrl,
            Statut = StatutLocationEnum.Active,
        };

        // Marquer le bien comme loué
        bien.Statut = StatutBienEnum.Loue;

        context.Locations.Add(location);
        await context.SaveChangesAsync();

        return await MapToDto(location.IdLocation);
    }

    public async Task<List<LocationResponseDto>> GetMesLocationsProprietaireAsync(
        int idProprietaire)
    {
        var locations = await context.Locations
            .Include(l => l.Bien)
            .Include(l => l.Locataire).ThenInclude(loc => loc.Utilisateur)
            .Where(l => l.Bien.IdProprietaire == idProprietaire)
            .OrderByDescending(l => l.DateDebut)
            .ToListAsync();

        return locations.Select(MapToLocationDto).ToList();
    }

    public async Task<List<LocationResponseDto>> GetMesLocationsLocataireAsync(
        int idLocataire)
    {
        var locations = await context.Locations
            .Include(l => l.Bien)
            .Include(l => l.Locataire).ThenInclude(loc => loc.Utilisateur)
            .Where(l => l.IdLocataire == idLocataire)
            .OrderByDescending(l => l.DateDebut)
            .ToListAsync();

        return locations.Select(MapToLocationDto).ToList();
    }

    public async Task<LocationResponseDto> TerminerLocationAsync(
        int idLocation, int idProprietaire)
    {
        var location = await context.Locations
            .Include(l => l.Bien)
            .Include(l => l.Locataire).ThenInclude(loc => loc.Utilisateur)
            .FirstOrDefaultAsync(l => l.IdLocation == idLocation)
            ?? throw new KeyNotFoundException("Location introuvable.");

        if (location.Bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Accès non autorisé.");

        location.Statut = StatutLocationEnum.Terminee;
        location.Bien.Statut = StatutBienEnum.Libre;

        await context.SaveChangesAsync();
        return MapToLocationDto(location);
    }

    private async Task<LocationResponseDto> MapToDto(int id)
    {
        var l = await context.Locations
            .Include(l => l.Bien)
            .Include(l => l.Locataire).ThenInclude(loc => loc.Utilisateur)
            .FirstAsync(l => l.IdLocation == id);
        return MapToLocationDto(l);
    }

    private static LocationResponseDto MapToLocationDto(Location l) => new()
    {
        IdLocation = l.IdLocation,
        IdBien = l.IdBien,
        TitreBien = l.Bien?.Titre ?? string.Empty,
        VilleBien = l.Bien?.Ville ?? string.Empty,
        AdresseBien = l.Bien?.Adresse ?? string.Empty,
        LoyerMensuel = l.Bien?.LoyerMensuel ?? 0,
        IdLocataire = l.IdLocataire,
        NomLocataire = l.Locataire?.Utilisateur != null
            ? $"{l.Locataire.Utilisateur.Prenom} {l.Locataire.Utilisateur.Nom}"
            : string.Empty,
        EmailLocataire = l.Locataire?.Utilisateur?.Email ?? string.Empty,
        Statut = l.Statut.ToString(),
        DateDebut = l.DateDebut,
        DateFin = l.DateFin,
        Caution = l.Caution,
        CautionPayee = l.CautionPayee,
        BailUrl = l.BailUrl,
    };
}
