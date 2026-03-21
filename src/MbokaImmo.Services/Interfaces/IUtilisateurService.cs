using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Utilisateurs;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface IUtilisateurService
{
    Task<PagedResultDto<UtilisateurResponseDto>> GetAllAsync(int page, int pageSize, string? role, string? search);
    Task<UtilisateurResponseDto?> GetByIdAsync(int id);
    Task<UtilisateurResponseDto> UpdateAsync(int id, UtilisateurUpdateDto dto);
    Task DeleteAsync(int id);
    Task ToggleCompteActifAsync(int id);
    Task ValiderKycAsync(int id);
}
