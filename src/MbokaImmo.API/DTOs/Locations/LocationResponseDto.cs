namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Locations;

public class LocationResponseDto
{
    public int IdLocation { get; set; }
    public int IdBien { get; set; }
    public string TitreBien { get; set; } = string.Empty;
    public string VilleBien { get; set; } = string.Empty;
    public string AdresseBien { get; set; } = string.Empty;
    public decimal LoyerMensuel { get; set; }
    public int IdLocataire { get; set; }
    public string NomLocataire { get; set; } = string.Empty;
    public string EmailLocataire { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public DateOnly DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
    public decimal Caution { get; set; }
    public bool CautionPayee { get; set; }
    public string? BailUrl { get; set; }
}