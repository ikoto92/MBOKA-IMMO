namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;

public class ArtisanAdminResponseDto
{
    public int IdArtisan { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string Specialite { get; set; } = string.Empty;
    public string? ZoneIntervention { get; set; }
    public float NoteMoyenne { get; set; }
    public int NbInterventions { get; set; }
    public bool Disponible { get; set; }
    public bool KycValide { get; set; }
    public DateTime DateInscription { get; set; }
}
