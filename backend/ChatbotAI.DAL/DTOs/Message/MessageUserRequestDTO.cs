using MediatR;

namespace ChatbotAI.DAL.DTOs.Message
{
    public class MessageUserRequestDTO : IRequest<IAsyncEnumerable<MessageAIResponseDTO>>
    {
        public string Message { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public Guid ChatId { get; set; }
    }
}
