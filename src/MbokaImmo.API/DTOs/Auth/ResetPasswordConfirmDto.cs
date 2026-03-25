namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;

public class ResetPasswordConfirmDto
{
    public string Token { get; set; } = string.Empty;
    public string NouveauMotDePasse { get; set; } = string.Empty;
}