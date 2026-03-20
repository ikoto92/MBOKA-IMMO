namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class PaiementIntervention
    {
        public int IdPaiementInterv { get; set; }
        public int IdPaiement { get; set; }
        public int IdArtisan { get; set; }
        public decimal MontantArtisan { get; set; }

        // Navigation
        public Paiement Paiement { get; set; } = null!;
        public Artisan Artisan { get; set; } = null!;
    }
}
