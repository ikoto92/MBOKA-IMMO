namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;

public class PaiementArtisanResponseDto
{
    public int IdPaiementInterv { get; set; }
    public decimal MontantArtisan { get; set; }
    public string TypeIntervention { get; set; } = string.Empty;
    public DateTime DatePaiement { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}
