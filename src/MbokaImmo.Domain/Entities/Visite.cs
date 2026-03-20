using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Visite
    {
        public int IdVisite { get; set; }
        public int IdBien { get; set; }
        public int IdAgent { get; set; }
        public int IdLocataire { get; set; }
        public DateTime DateVisite { get; set; }
        public string? CompteRendu { get; set; }
        public StatutVisiteEnum Statut { get; set; } = StatutVisiteEnum.Planifiee;

        // Navigation
        public Bien Bien { get; set; } = null!;
        public Agent Agent { get; set; } = null!;
        public Locataire Locataire { get; set; } = null!;
        public ICollection<Photo> Photos { get; set; } = [];
    }
}
