using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Virement
    {
        public int IdVirement { get; set; }
        public int IdProprio { get; set; }
        public string IbanDestination { get; set; } = string.Empty;
        public decimal MontantNet { get; set; }
        public decimal Commission { get; set; } = 0m;
        public StatutVirementEnum Statut { get; set; } = StatutVirementEnum.EnAttente;
        public DateTime? DateVirement { get; set; }
        public string? ReferenceBanque { get; set; }
        public string? Operateur { get; set; }

        // Navigation
        public Proprietaire Proprietaire { get; set; } = null!;
    }
}
