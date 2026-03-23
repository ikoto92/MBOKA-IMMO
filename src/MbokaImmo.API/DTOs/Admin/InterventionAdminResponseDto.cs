namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;

public class InterventionAdminResponseDto
{
    public int IdIntervention { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Urgence { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public string TitreBien { get; set; } = string.Empty;
    public string VilleBien { get; set; } = string.Empty;
    public string? NomArtisan { get; set; }
    public decimal? DevisMontant { get; set; }
    public bool DevisValide { get; set; }
    public DateTime DateSignalement { get; set; }
    public DateTime? DateIntervention { get; set; }
}
