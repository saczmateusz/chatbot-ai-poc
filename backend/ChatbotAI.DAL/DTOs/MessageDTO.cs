using ChatbotAI.Core.Domain;
using ChatbotAI.Core.Enums;

namespace ChatbotAI.DAL.DTOs
{
    public class MessageDTO
    {

        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

        public string Content { get; set; } = string.Empty;
        public Guid ChatId { get; set; }
        public virtual Chat Chat { get; set; } = null!;
        public string Sender { get; set; } = string.Empty;

        public ReactionType Reaction { get; set; }
    }
}
