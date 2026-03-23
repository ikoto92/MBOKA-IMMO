using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Candidatures;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface ICandidatureService
{
    Task<CandidatureResponseDto> SoumettreCandidatureAsync(CandidatureCreateDto dto, int idLocataire);
    Task<List<CandidatureResponseDto>> GetMesCandidaturesAsync(int idLocataire);
    Task<List<CandidatureResponseDto>> GetCandidaturesBienAsync(int idBien, int idProprietaire);
    Task<CandidatureResponseDto> DeciderCandidatureAsync(int idCandidature, CandidatureDecisionDto dto, int idProprietaire);
}
