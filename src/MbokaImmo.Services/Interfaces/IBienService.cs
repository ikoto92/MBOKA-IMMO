using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Common;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface IBienService
{
    Task<PagedResultDto<BienResponseDto>> GetAllAsync(int page, int pageSize, string? ville, string? type, decimal? loyerMax);
    Task<BienResponseDto?> GetByIdAsync(int idBien);
    Task<List<BienResponseDto>> GetMesBiensAsync(int idProprietaire);
    Task<BienResponseDto> CreateAsync(BienCreateDto dto, int idProprietaire);
    Task<BienResponseDto> UpdateAsync(int idBien, BienUpdateDto dto, int idProprietaire);
    Task DeleteAsync(int idBien, int idProprietaire);
    Task<List<string>> UploadPhotosAsync(int idBien, List<IFormFile> photos, int idProprietaire);
    Task<BienResponseDto> ValiderAsync(int idBien);
}
