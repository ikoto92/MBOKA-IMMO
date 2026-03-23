namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Admin;

public class BienAdminResponseDto
{
    public int IdBien { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal LoyerMensuel { get; set; }
    public string Statut { get; set; } = string.Empty;
    public bool Valide { get; set; }
    public string NomProprietaire { get; set; } = string.Empty;
    public string EmailProprietaire { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
    public int NbPhotos { get; set; }
}
