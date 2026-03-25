using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Locations;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface ILocationService
{
    Task<LocationResponseDto> CreerBailAsync(LocationCreateDto dto, int idProprietaire);
    Task<List<LocationResponseDto>> GetMesLocationsProprietaireAsync(int idProprietaire);
    Task<List<LocationResponseDto>> GetMesLocationsLocataireAsync(int idLocataire);
    Task<LocationResponseDto> TerminerLocationAsync(int idLocation, int idProprietaire);
}
