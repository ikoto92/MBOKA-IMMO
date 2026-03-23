using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities;

public class Candidature
{
    public int IdCandidature { get; set; }
    public int IdBien { get; set; }
    public int IdLocataire { get; set; }

    public string? MessageMotivation { get; set; }
    public decimal RevenusMenuels { get; set; }
    public string? PieceIdentiteUrl { get; set; }
    public string? JustificatifUrl { get; set; }

    public StatutCandidatureEnum Statut { get; set; }
    public DateTime DateCandidature { get; set; }

    public Bien Bien { get; set; } = null!;
    public Locataire Locataire { get; set; } = null!;
}