using ChatbotAI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

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

        //public Guid? CreatedBy { get; set; }

        //public Guid? ModifiedBy { get; set; }

        // ---------------------------------------

        public string Name { get; set; } = string.Empty;

        virtual public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
