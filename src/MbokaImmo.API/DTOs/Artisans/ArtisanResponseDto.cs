namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Artisans;

public class ArtisanResponseDto
{
    public int IdArtisan { get; set; }
    public int IdUser { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string Specialite { get; set; } = string.Empty;
    public string? ZoneIntervention { get; set; }
    public float NoteMoyenne { get; set; }
    public int NbInterventions { get; set; }
    public bool Disponible { get; set; }
    public decimal? TarifHoraire { get; set; }
}