using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Location
    {
        public int IdLocation { get; set; }  // ← vérifiez que c'est bien là
        public int IdBien { get; set; }
        public int IdLocataire { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly? DateFin { get; set; }
        public decimal LoyerMensuel { get; set; }
        public decimal Caution { get; set; }
        public bool CautionPayee { get; set; } = false;
        public StatutLocationEnum Statut { get; set; } = StatutLocationEnum.Active;
        public string? BailUrl { get; set; }
        public DateTime? DateSignature { get; set; }

        // Navigation
        public Bien Bien { get; set; } = null!;
        public Locataire Locataire { get; set; } = null!;
        public ICollection<Paiement> Paiements { get; set; } = [];
        public ICollection<Intervention> Interventions { get; set; } = [];
    }
}
