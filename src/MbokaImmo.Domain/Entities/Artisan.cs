namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Artisan
    {
        public int IdArtisan { get; set; }
        public int IdUser { get; set; }
        public string Specialite { get; set; } = string.Empty;
        public string? ZoneIntervention { get; set; }
        public float NoteMoyenne { get; set; } = 0f;
        public int NbInterventions { get; set; } = 0;
        public bool Disponible { get; set; } = true;
        public decimal? TarifHoraire { get; set; }

        // Navigation
        public Utilisateur Utilisateur { get; set; } = null!;
        public ICollection<Intervention> Interventions { get; set; } = [];
        public ICollection<PaiementIntervention> PaiementsIntervention { get; set; } = [];
    }
}
