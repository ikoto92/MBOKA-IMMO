namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;

public class MissionResponseDto
{
    public int IdIntervention { get; set; }
    public int IdLocation { get; set; }
    public string TitreBien { get; set; } = string.Empty;
    public string VilleBien { get; set; } = string.Empty;
    public string AdresseBien { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Urgence { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public DateTime DateSignalement { get; set; }
    public DateTime? DateIntervention { get; set; }
    public decimal? DevisMontant { get; set; }
    public bool DevisValide { get; set; }
    public string? FactureUrl { get; set; }
    public int? NoteLocataire { get; set; }
}
