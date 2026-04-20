namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class RefreshToken
    {
        public int IdRefreshToken { get; set; }
        public string Token { get; set; } = string.Empty;
        public int IdUser { get; set; }
        public DateTime DateExpiration { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public DateTime? DateRevocation { get; set; }

        public Utilisateur Utilisateur { get; set; } = null!;

        public bool EstActif => DateRevocation is null && DateExpiration > DateTime.UtcNow;
    }
}
