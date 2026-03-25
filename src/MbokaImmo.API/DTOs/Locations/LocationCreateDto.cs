namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Locations;

public class LocationCreateDto
{
    public int IdBien { get; set; }
    public int IdLocataire { get; set; }
    public DateOnly DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
    public decimal Caution { get; set; }
    public bool CautionPayee { get; set; } = false;
    public string? BailUrl { get; set; }
}