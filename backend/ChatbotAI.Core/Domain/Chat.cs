using System.ComponentModel.DataAnnotations;
using ChatbotAI.Core.Interfaces;

namespace ChatbotAI.Core.Domain
{
    public class Chat : IDomainEntity
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

        public string Name { get; set; } = string.Empty;

        virtual public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
