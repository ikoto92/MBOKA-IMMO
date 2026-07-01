using System.Security.Claims;
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBOKA_IMMO.src.MbokaImmo.API.Controllers;

[ApiController]
[Route("api/v1/biens")]
[Produces("application/json")]
public class BiensController(IBienService bienService) : ControllerBase
{
    private int GetProprietaireId()
        => int.Parse(User.FindFirstValue("proprietaireId")
            ?? throw new UnauthorizedAccessException());

   
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? ville = null,
        [FromQuery] string? type = null,
        [FromQuery] decimal? loyerMax = null)
    {
        var result = await bienService.GetAllAsync(page, pageSize, ville, type, loyerMax);
        return Ok(result);
    }

 
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var bien = await bienService.GetByIdAsync(id);
        return bien is null ? NotFound() : Ok(bien);
    }


    [HttpGet("mes-biens")]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> GetMesBiens()
    {
        try
        {
            var result = await bienService.GetMesBiensAsync(GetProprietaireId());
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }


    [HttpPost]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> Create([FromBody] BienCreateDto dto)
    {
        try
        {
            var result = await bienService.CreateAsync(dto, GetProprietaireId());
            return CreatedAtAction(nameof(GetById), new { id = result.IdBien }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Proprio")]
    public async Task<IActionResult> Update(int id, [FromBody] BienUpdateDto dto)
    {
        try
        {
            var result = await bienService.UpdateAsync(id, dto, GetProprietaireId());
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }  
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Proprio,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await bienService.DeleteAsync(id, GetProprietaireId());
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }


    [HttpPost("{id:int}/photos")]
    [Authorize(Roles = "Proprio")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPhotos(
        int id, [FromForm] List<IFormFile> photos)
    {
        try
        {
            var urls = await bienService.UploadPhotosAsync(id, photos, GetProprietaireId());
            return Ok(new { urls });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

   
    [HttpPut("{id:int}/valider")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Valider(int id)
    {
        try
        {
            var result = await bienService.ValiderAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}

