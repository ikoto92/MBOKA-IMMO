using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Candidature
    {
        public int IdCandidature { get; set; }
        public int IdBien { get; set; }
        public int IdLocataire { get; set; }
        public DateTime DateCandidature { get; set; } = DateTime.UtcNow;
        public StatutCandidatureEnum Statut { get; set; } = StatutCandidatureEnum.EnAttente;
        public string? MessageCandidat { get; set; }

        // Navigation
        public Bien Bien { get; set; } = null!;
        public Locataire Locataire { get; set; } = null!;
    }
}
