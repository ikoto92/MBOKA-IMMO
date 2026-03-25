using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Virements;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface IVirementService
{
    Task<VirementResponseDto> DemanderVirementAsync(VirementCreateDto dto, int idProprietaire);
    Task<List<VirementResponseDto>> GetMesVirementsAsync(int idProprietaire);
}