namespace ChatbotAI.DAL.DTOs.Message
{
    public class MessageAIResponseDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}
