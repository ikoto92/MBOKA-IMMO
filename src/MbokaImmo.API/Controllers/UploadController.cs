using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/upload")]
[Authorize]
[ApiExplorerSettings(IgnoreApi = true)] // ← cache tout le controller à Swagger
public class UploadController(IStorageService storage) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "Fichier manquant." });

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest(new { error = "Fichier trop volumineux (max 10 MB)." });

        var allowedTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { error = "Type de fichier non autorisé." });

        var url = await storage.UploadAsync(file, "documents");
        return Ok(new { url });
    }
}