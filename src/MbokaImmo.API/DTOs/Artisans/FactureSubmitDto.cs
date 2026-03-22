namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;

public class FactureSubmitDto
{
    public decimal Montant { get; set; }
    public string FactureUrl { get; set; } = string.Empty;
}