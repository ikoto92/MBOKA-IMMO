using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Utilisateur
    {
        public int IdUser { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string? PaysResidence { get; set; }
        public string? VilleResidence { get; set; }
        public string? PieceIdentite { get; set; }
        public bool KycValide { get; set; } = false;
        public RoleEnum Role { get; set; }
        public DateTime DateInscription { get; set; } = DateTime.UtcNow;
        public bool CompteActif { get; set; } = true;

        // Navigation
        public Proprietaire? Proprietaire { get; set; }
        public Locataire? Locataire { get; set; }
        public Agent? Agent { get; set; }
        public Artisan? Artisan { get; set; }
        public ICollection<Notification> Notifications { get; set; } = [];
        public ICollection<Favori> Favoris { get; set; } = [];
        public ICollection<Message> MessagesEnvoyes { get; set; } = [];
        public ICollection<Conversation> Conversations1 { get; set; } = [];
        public ICollection<Conversation> Conversations2 { get; set; } = [];
    }
}
