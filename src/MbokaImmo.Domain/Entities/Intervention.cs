using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Intervention
    {
        public int IdIntervention { get; set; }
        public int IdLocation { get; set; }
        public int? IdArtisan { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public UrgenceEnum Urgence { get; set; } = UrgenceEnum.Moyenne;
        public StatutInterventionEnum Statut { get; set; } = StatutInterventionEnum.Signale;
        public DateTime DateSignalement { get; set; } = DateTime.UtcNow;
        public DateTime? DateIntervention { get; set; }
        public decimal? DevisMontant { get; set; }
        public bool DevisValide { get; set; } = false;
        public string? FactureUrl { get; set; }
        public int? NoteLocataire { get; set; }
        public string? Commentaire { get; set; }

        // Navigation
        public Location Location { get; set; } = null!;
        public Artisan? Artisan { get; set; }
        public ICollection<Photo> Photos { get; set; } = [];
    }
}
