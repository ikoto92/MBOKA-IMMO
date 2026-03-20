namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Message
    {
        public int IdMessage { get; set; }
        public int IdConversation { get; set; }
        public int ExpediteurId { get; set; }
        public string Contenu { get; set; } = string.Empty;
        public string? PieceJointe { get; set; }
        public bool Lu { get; set; } = false;
        public DateTime DateEnvoi { get; set; } = DateTime.UtcNow;

        // Navigation
        public Conversation Conversation { get; set; } = null!;
        public Utilisateur Expediteur { get; set; } = null!;
    }
}
