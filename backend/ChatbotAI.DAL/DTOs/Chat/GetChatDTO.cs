namespace ChatbotAI.DAL.DTOs.Chat
{
    public class GetChatDTO
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
