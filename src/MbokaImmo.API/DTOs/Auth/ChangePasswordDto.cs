namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;

public class ChangePasswordDto
{
    public string AncienMotDePasse { get; set; } = string.Empty;
    public string NouveauMotDePasse { get; set; } = string.Empty;
}