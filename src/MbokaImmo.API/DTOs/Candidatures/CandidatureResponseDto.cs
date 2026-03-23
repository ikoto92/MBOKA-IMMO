namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Candidatures;
public class CandidatureResponseDto
{
    public int IdCandidature { get; set; }
    public int IdBien { get; set; }
    public string TitreBien { get; set; } = string.Empty;
    public string VilleBien { get; set; } = string.Empty;
    public decimal LoyerMensuel { get; set; }
    public int IdLocataire { get; set; }
    public string NomLocataire { get; set; } = string.Empty;
    public string PrenomLocataire { get; set; } = string.Empty;
    public string EmailLocataire { get; set; } = string.Empty;
    public string? TelephoneLocataire { get; set; }
    public string? MessageMotivation { get; set; }
    public decimal RevenusMenuels { get; set; }
    public string? PieceIdentiteUrl { get; set; }
    public string? JustificatifUrl { get; set; }
    public string Statut { get; set; } = string.Empty;
    public DateTime DateCandidature { get; set; }
}
