namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;

public class BienCreateDto
{
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string? Quartier { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Surface { get; set; }
    public int Pieces { get; set; }
    public decimal LoyerMensuel { get; set; }
    public int CautionMois { get; set; } = 2;
    public string? Video { get; set; }
    public DateOnly? Disponibilite { get; set; }
}
