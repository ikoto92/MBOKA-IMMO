namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth
{
    public class UserDto
    {
        public int IdUser { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool KycValide { get; set; }
    }
}
