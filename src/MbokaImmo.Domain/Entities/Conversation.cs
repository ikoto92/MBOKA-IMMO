namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Conversation
    {
        public int IdConversation { get; set; }
        public int Participant1Id { get; set; }
        public int Participant2Id { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public DateTime? DernierMessage { get; set; }

        // Navigation
        public Utilisateur Participant1 { get; set; } = null!;
        public Utilisateur Participant2 { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = [];
    }
}
