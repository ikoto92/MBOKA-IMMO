namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string? PaysResidence { get; set; }
        public string? VilleResidence { get; set; }
        public string Role { get; set; } = "Locataire";
    }
}
