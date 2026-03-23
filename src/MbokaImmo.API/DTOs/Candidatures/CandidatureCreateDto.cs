namespace MBOKA_IMMO.src.MbokaImmo.API.DTOs.Candidatures;

public class CandidatureCreateDto
{
    public int IdBien { get; set; }
    public string? MessageMotivation { get; set; }
    public decimal RevenusMenuels { get; set; }
    public string? PieceIdentiteUrl { get; set; }
    public string? JustificatifUrl { get; set; }
}