using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Virements;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class VirementService(AppDbContext context) : IVirementService
{
    public async Task<VirementResponseDto> DemanderVirementAsync(
        VirementCreateDto dto, int idProprietaire)
    {
        var virement = new Virement
        {
            IdProprio = idProprietaire,
            IbanDestination = dto.Iban,        // ← IbanDestination au lieu de Iban
            MontantNet = dto.Montant,     // ← MontantNet au lieu de Montant
            Commission = 0m,
            Operateur = dto.BanqueNom,   // ← Operateur au lieu de BanqueNom
            Statut = StatutVirementEnum.EnAttente,
            DateVirement = DateTime.UtcNow,
            ReferenceBanque = $"VIR-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
        };

        context.Virements.Add(virement);
        await context.SaveChangesAsync();

        return MapToDto(virement);
    }

    public async Task<List<VirementResponseDto>> GetMesVirementsAsync(int idProprietaire)
    {
        var virements = await context.Virements
            .Where(v => v.IdProprio == idProprietaire)
            .OrderByDescending(v => v.DateVirement)
            .ToListAsync();

        return virements.Select(MapToDto).ToList();
    }

    private static VirementResponseDto MapToDto(Virement v) => new()
    {
        IdVirement = v.IdVirement,
        Montant = v.MontantNet,           // ← MontantNet
        Iban = v.IbanDestination,      // ← IbanDestination
        BanqueNom = v.Operateur ?? string.Empty, // ← Operateur
        Motif = null,
        Statut = v.Statut.ToString(),
        DateVirement = v.DateVirement ?? DateTime.UtcNow,
        Reference = v.ReferenceBanque,      // ← ReferenceBanque
    };
}