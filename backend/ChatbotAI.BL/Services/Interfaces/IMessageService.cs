using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.DTOs.Message;

namespace ChatbotAI.BL.Services.Interfaces
{
    public interface IMessageService
    {
        IAsyncEnumerable<MessageAIResponseDTO> GenerateResponseStreamAsync(string message, Guid chatId, CancellationToken cancellationToken);
        Task<Message> CreateAsync(MessageUserRequestDTO message, CancellationToken cancellationToken = default);
        Task<Message> CreateAIMessageAsync(string content, Guid chatId, Guid messageId, CancellationToken cancellationToken = default);
        Task UpdateAsync(MessageDTO message, CancellationToken cancellationToken = default);
        Task<List<MessageDTO>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken = default);
    }
}
