namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;

public class BienUpdateDto
{
    public string? Titre { get; set; }
    public string? Description { get; set; }
    public string? Adresse { get; set; }
    public string? Ville { get; set; }
    public string? Quartier { get; set; }
    public string? Type { get; set; }
    public int? Surface { get; set; }
    public int? Pieces { get; set; }
    public decimal? LoyerMensuel { get; set; }
    public int? CautionMois { get; set; }
    public string? Video { get; set; }
    public string? Statut { get; set; }
    public DateOnly? Disponibilite { get; set; }
}
