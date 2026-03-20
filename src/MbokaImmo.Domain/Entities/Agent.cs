namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Agent
    {
        public int IdAgent { get; set; }
        public int IdUser { get; set; }
        public string? ZoneIntervention { get; set; }
        public int NbVisites { get; set; } = 0;

        // Navigation
        public Utilisateur Utilisateur { get; set; } = null!;
        public ICollection<Visite> Visites { get; set; } = [];
    }
}
