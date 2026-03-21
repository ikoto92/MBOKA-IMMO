namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Biens;

public class BienResponseDto
{
    public int IdBien { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string? Quartier { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Surface { get; set; }
    public int Pieces { get; set; }
    public decimal LoyerMensuel { get; set; }
    public int CautionMois { get; set; }
    public string Statut { get; set; } = string.Empty;
    public bool Valide { get; set; }
    public DateTime DateCreation { get; set; }
}

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
}

public class BienUpdateDto
{
    public string? Titre { get; set; }
    public string? Description { get; set; }
    public decimal? LoyerMensuel { get; set; }
    public string? Statut { get; set; }
    public bool? Valide { get; set; }
}