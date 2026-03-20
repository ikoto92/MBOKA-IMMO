namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Proprietaire
    {
        public int IdProprio { get; set; }
        public int IdUser { get; set; }
        public string? CompteBancaire { get; set; }
        public string? Iban { get; set; }

        // Navigation
        public Utilisateur Utilisateur { get; set; } = null!;
        public ICollection<Bien> Biens { get; set; } = [];
        public ICollection<Virement> Virements { get; set; } = [];
    }
}
