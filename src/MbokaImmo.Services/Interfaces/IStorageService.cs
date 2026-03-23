namespace MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, string folder);
    Task DeleteAsync(string url);
}