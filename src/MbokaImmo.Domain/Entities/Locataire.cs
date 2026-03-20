namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Locataire
    {
        public int IdLocataire { get; set; }
        public int IdUser { get; set; }
        public int ScoreCreditworthiness { get; set; } = 0;

        // Navigation
        public Utilisateur Utilisateur { get; set; } = null!;
        public ICollection<Location> Locations { get; set; } = [];
        public ICollection<Candidature> Candidatures { get; set; } = [];
        public ICollection<Visite> Visites { get; set; } = [];
    }
}
