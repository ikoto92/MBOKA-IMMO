using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;

namespace MBOKA_IMMO.src.MbokaImmo.Infrastructure.ExternalServices.Storage;

public class LocalStorageService(IWebHostEnvironment env) : IStorageService
{
    public async Task<string> UploadAsync(IFormFile file, string folder)
    {
        // WebRootPath peut être null si wwwroot n'existe pas
        var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsDir = Path.Combine(webRoot, "uploads", folder);
        Directory.CreateDirectory(uploadsDir);

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{folder}/{fileName}";
    }

    public Task DeleteAsync(string url)
    {
        var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var filePath = Path.Combine(webRoot, url.TrimStart('/'));
        if (File.Exists(filePath)) File.Delete(filePath);
        return Task.CompletedTask;
    }
}