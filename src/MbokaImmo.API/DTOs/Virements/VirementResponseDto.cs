namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Virements;

public class VirementResponseDto
{
    public int IdVirement { get; set; }
    public decimal Montant { get; set; }
    public string Iban { get; set; } = string.Empty;
    public string BanqueNom { get; set; } = string.Empty;
    public string? Motif { get; set; }
    public string Statut { get; set; } = string.Empty;
    public DateTime DateVirement { get; set; }
    public string? Reference { get; set; }
}