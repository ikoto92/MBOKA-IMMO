namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class PaiementLoyer
    {
        public int IdPaiementLoyer { get; set; }
        public int IdPaiement { get; set; }
        public DateOnly MoisConcerne { get; set; }

        // Navigation
        public Paiement Paiement { get; set; } = null!;
    }
}
