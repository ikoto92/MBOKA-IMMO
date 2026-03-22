using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface IArtisanService
{
    Task<ArtisanResponseDto> GetProfilAsync(int idUser);
    Task<ArtisanResponseDto> UpdateProfilAsync(int idUser, ArtisanUpdateDto dto);
    Task<IEnumerable<MissionResponseDto>> GetMessMissionsAsync(int idArtisan);
    Task<MissionResponseDto> SoumettreDevisAsync(int idIntervention, int idArtisan, DevisSubmitDto dto);
    Task<MissionResponseDto> UpdateStatutAsync(int idIntervention, int idArtisan, StatutUpdateDto dto);
    Task<MissionResponseDto> EnvoyerFactureAsync(int idIntervention, int idArtisan, FactureSubmitDto dto);
    Task<IEnumerable<PaiementArtisanResponseDto>> GetMesPaiementsAsync(int idArtisan);
}

