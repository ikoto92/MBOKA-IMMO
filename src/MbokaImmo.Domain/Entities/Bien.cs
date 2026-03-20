using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Bien
    {
        public int IdBien { get; set; }
        public int IdProprietaire { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string? Quartier { get; set; }
        public TypeBienEnum Type { get; set; }
        public int Surface { get; set; }
        public int Pieces { get; set; }
        public decimal LoyerMensuel { get; set; }
        public int CautionMois { get; set; } = 2;
        public DateOnly? Disponibilite { get; set; }
        public StatutBienEnum Statut { get; set; } = StatutBienEnum.Libre;
        public string? Video { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public bool Valide { get; set; } = false;

        // Navigation
        public Proprietaire Proprietaire { get; set; } = null!;
        public ICollection<Photo> Photos { get; set; } = [];
        public ICollection<Location> Locations { get; set; } = [];
        public ICollection<Candidature> Candidatures { get; set; } = [];
        public ICollection<Visite> Visites { get; set; } = [];
        public ICollection<Favori> Favoris { get; set; } = [];
    }
}
