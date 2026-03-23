using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface IAdminService
{
    Task<StatistiquesDto> GetStatistiquesAsync();
    Task<List<BienAdminResponseDto>> GetBiensEnAttenteAsync();
    Task<List<BienAdminResponseDto>> GetAllBiensAsync();
    Task<BienResponseDto> ValiderBienAsync(int idBien);
    Task<BienResponseDto> RefuserBienAsync(int idBien);
    Task<List<ArtisanAdminResponseDto>> GetAllArtisansAsync();
    Task<ArtisanAdminResponseDto> ValiderArtisanKycAsync(int idArtisan);
    Task<List<InterventionAdminResponseDto>> GetAllInterventionsAsync();
    Task<InterventionAdminResponseDto> AssignerArtisanAsync(int idIntervention, int idArtisan);
    Task<InterventionAdminResponseDto> ValiderDevisAsync(int idIntervention, bool valide);
}

