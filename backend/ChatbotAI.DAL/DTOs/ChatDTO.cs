using ChatbotAI.Core.Domain;

namespace ChatbotAI.DAL.DTOs
{
    public class ChatDTO
    {

        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        virtual public ICollection<Message> Messages { get; set; } = [];
    }
}
