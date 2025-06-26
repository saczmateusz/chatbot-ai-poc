using System.ComponentModel.DataAnnotations;
using ChatbotAI.Core.Enums;
using ChatbotAI.Core.Interfaces;

namespace ChatbotAI.Core.Domain
{
    public class Message : IDomainEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        // ---------------------------------------

        public string Content { get; set; } = string.Empty;
        public Guid ChatId { get; set; }
        public virtual Chat Chat { get; set; } = null!;
        public string Sender { get; set; } = string.Empty;

        public ReactionType Reaction { get; set; }
    }
}
