using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;
using MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class BienService(
    IBienRepository bienRepo,
    IStorageService storage) : IBienService
{
    // ── Lister tous les biens ────────────────────────────────────
    public async Task<PagedResultDto<BienResponseDto>> GetAllAsync(
        int page, int pageSize, string? ville, string? type, decimal? loyerMax)
    {
        var (items, total) = await bienRepo.GetAllAsync(page, pageSize, ville, type, loyerMax);
        return new PagedResultDto<BienResponseDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    // ── Voir un bien ─────────────────────────────────────────────
    public async Task<BienResponseDto?> GetByIdAsync(int idBien)
    {
        var bien = await bienRepo.GetByIdAsync(idBien);
        return bien is null ? null : MapToDto(bien);
    }

    // ── Mes biens ────────────────────────────────────────────────
    public async Task<List<BienResponseDto>> GetMesBiensAsync(int idProprietaire)
    {
        var biens = await bienRepo.GetByProprietaireAsync(idProprietaire);
        return biens.Select(MapToDto).ToList();
    }

    // ── Publier un bien ──────────────────────────────────────────
    public async Task<BienResponseDto> CreateAsync(BienCreateDto dto, int idProprietaire)
    {
        if (!Enum.TryParse<TypeBienEnum>(dto.Type, true, out var type))
            throw new ArgumentException($"Type de bien invalide : {dto.Type}");

        var bien = new Bien
        {
            IdProprietaire = idProprietaire,
            Titre = dto.Titre,
            Description = dto.Description,
            Adresse = dto.Adresse,
            Ville = dto.Ville,
            Quartier = dto.Quartier,
            Type = type,
            Surface = dto.Surface,
            Pieces = dto.Pieces,
            LoyerMensuel = dto.LoyerMensuel,
            CautionMois = dto.CautionMois,
            Video = dto.Video,
            Disponibilite = dto.Disponibilite,
            Statut = StatutBienEnum.Libre,
            Valide = false,
            DateCreation = DateTime.UtcNow,
        };

        await bienRepo.AddAsync(bien);
        await bienRepo.SaveChangesAsync();

        return MapToDto(bien);
    }

    // ── Modifier un bien ─────────────────────────────────────────
    public async Task<BienResponseDto> UpdateAsync(
        int idBien, BienUpdateDto dto, int idProprietaire)
    {
        var bien = await bienRepo.GetByIdAsync(idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        if (bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Vous n'êtes pas propriétaire de ce bien.");

        if (dto.Titre is not null) bien.Titre = dto.Titre;
        if (dto.Description is not null) bien.Description = dto.Description;
        if (dto.Adresse is not null) bien.Adresse = dto.Adresse;
        if (dto.Ville is not null) bien.Ville = dto.Ville;
        if (dto.Quartier is not null) bien.Quartier = dto.Quartier;
        if (dto.Surface is not null) bien.Surface = dto.Surface.Value;
        if (dto.Pieces is not null) bien.Pieces = dto.Pieces.Value;
        if (dto.LoyerMensuel is not null) bien.LoyerMensuel = dto.LoyerMensuel.Value;
        if (dto.CautionMois is not null) bien.CautionMois = dto.CautionMois.Value;
        if (dto.Video is not null) bien.Video = dto.Video;
        if (dto.Disponibilite is not null) bien.Disponibilite = dto.Disponibilite;

        if (dto.Type is not null && Enum.TryParse<TypeBienEnum>(dto.Type, true, out var type))
            bien.Type = type;

        if (dto.Statut is not null && Enum.TryParse<StatutBienEnum>(dto.Statut, true, out var statut))
            bien.Statut = statut;

        await bienRepo.UpdateAsync(bien);
        await bienRepo.SaveChangesAsync();

        return MapToDto(bien);
    }

    // ── Supprimer un bien ────────────────────────────────────────
    public async Task DeleteAsync(int idBien, int idProprietaire)
    {
        var bien = await bienRepo.GetByIdAsync(idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        if (bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Vous n'êtes pas propriétaire de ce bien.");

        if (bien.Statut == StatutBienEnum.Loue)
            throw new InvalidOperationException("Impossible de supprimer un bien loué.");

        await bienRepo.DeleteAsync(bien);
        await bienRepo.SaveChangesAsync();
    }

    // ── Upload photos ────────────────────────────────────────────
    public async Task<List<string>> UploadPhotosAsync(
        int idBien, List<IFormFile> photos, int idProprietaire)
    {
        var bien = await bienRepo.GetByIdAsync(idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        if (bien.IdProprietaire != idProprietaire)
            throw new UnauthorizedAccessException("Vous n'êtes pas propriétaire de ce bien.");

        if (photos.Count > 10)
            throw new ArgumentException("Maximum 10 photos par bien.");

        var urls = new List<string>();
        foreach (var photo in photos)
        {
            var url = await storage.UploadAsync(photo, $"biens/{idBien}");
            urls.Add(url);

            bien.Photos.Add(new Photo
            {
                EntiteId = idBien,
                EntiteType = Domain.Enums.EntiteTypeEnum.Bien,
                Url = url,
                Ordre = bien.Photos.Count,
                DateUpload = DateTime.UtcNow,
            });
        }

        await bienRepo.UpdateAsync(bien);
        await bienRepo.SaveChangesAsync();

        return urls;
    }

    // ── Valider un bien (admin) ──────────────────────────────────
    public async Task<BienResponseDto> ValiderAsync(int idBien)
    {
        var bien = await bienRepo.GetByIdAsync(idBien)
            ?? throw new KeyNotFoundException("Bien introuvable.");

        bien.Valide = true;
        await bienRepo.UpdateAsync(bien);
        await bienRepo.SaveChangesAsync();

        return MapToDto(bien);
    }

    // ── Mapper ───────────────────────────────────────────────────
    private static BienResponseDto MapToDto(Bien b) => new()
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
}
