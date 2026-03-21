namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Utilisateurs;

public class UtilisateurResponseDto
{
    public int IdUser { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string? PaysResidence { get; set; }
    public string? VilleResidence { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool KycValide { get; set; }
    public bool CompteActif { get; set; }
    public DateTime DateInscription { get; set; }
}

public class UtilisateurUpdateDto
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Telephone { get; set; }
    public string? PaysResidence { get; set; }
    public string? VilleResidence { get; set; }
    public string? PieceIdentite { get; set; }
}