namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Favori
    {
        public int IdFavori { get; set; }
        public int IdUser { get; set; }
        public int IdBien { get; set; }
        public DateTime DateAjout { get; set; } = DateTime.UtcNow;

        // Navigation
        public Utilisateur Utilisateur { get; set; } = null!;
        public Bien Bien { get; set; } = null!;
    }
}
