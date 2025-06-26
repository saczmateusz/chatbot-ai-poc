using System.ComponentModel.DataAnnotations;

namespace ChatbotAI.Core.Interfaces
{
    public interface IDomainEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
