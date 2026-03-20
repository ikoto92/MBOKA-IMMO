using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Paiement
    {
        public int IdPaiement { get; set; }
        public int IdLocation { get; set; }
        public TypePaiementEnum Type { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; } = DateTime.UtcNow;
        public ModePaiementEnum Mode { get; set; }
        public string? Operateur { get; set; }
        public string Reference { get; set; } = Guid.NewGuid().ToString("N");
        public StatutPaiementEnum Statut { get; set; } = StatutPaiementEnum.EnAttente;
        public decimal Commission { get; set; } = 0m;
        public decimal? MontantNet { get; set; }

        // Navigation
        public Location Location { get; set; } = null!;
        public PaiementLoyer? PaiementLoyer { get; set; }
        public PaiementIntervention? PaiementIntervention { get; set; }
    }
}
