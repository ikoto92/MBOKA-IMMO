namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;

public class ArtisanUpdateDto
{
    public string? Specialite { get; set; }
    public string? ZoneIntervention { get; set; }
    public bool? Disponible { get; set; }
    public decimal? TarifHoraire { get; set; }
    public string? Telephone { get; set; }
}
