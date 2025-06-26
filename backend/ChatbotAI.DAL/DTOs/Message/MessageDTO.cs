using ChatbotAI.Core.Enums;

namespace ChatbotAI.DAL.DTOs.Message
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public ReactionType Reaction { get; set; }
    }
}
