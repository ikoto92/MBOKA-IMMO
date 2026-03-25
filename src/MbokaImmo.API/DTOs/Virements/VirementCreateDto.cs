namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Virements;

public class VirementCreateDto
{
    public decimal Montant { get; set; }
    public string Iban { get; set; } = string.Empty;
    public string BanqueNom { get; set; } = string.Empty;
    public string? Motif { get; set; }
}