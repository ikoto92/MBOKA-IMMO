namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;

public class StatistiquesDto
{
    public int TotalUtilisateurs { get; set; }
    public int TotalProprietaires { get; set; }
    public int TotalLocataires { get; set; }
    public int TotalArtisans { get; set; }
    public int TotalBiens { get; set; }
    public int BiensValides { get; set; }
    public int BiensEnAttente { get; set; }
    public int BiensLoues { get; set; }
    public int TotalLocations { get; set; }
    public int LocationsActives { get; set; }
    public int TotalPaiements { get; set; }
    public decimal RevenusTotaux { get; set; }
    public decimal CommissionsTotales { get; set; }
    public int TotalInterventions { get; set; }
    public int InterventionsEnCours { get; set; }
}