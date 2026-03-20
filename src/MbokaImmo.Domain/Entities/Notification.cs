namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Notification
    {
        public int IdNotification { get; set; }
        public int IdUser { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Contenu { get; set; } = string.Empty;
        public string? Lien { get; set; }
        public bool Lu { get; set; } = false;
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        // Navigation
        public Utilisateur Utilisateur { get; set; } = null!;
    }
}
