using ChatbotAI.DAL.DTOs;

namespace ChatbotAI.BL.Services.Interfaces
{
    public interface IMessageService
    {
        Task CreateAsync(MessageDTO message, CancellationToken cancellationToken = default);
        Task UpdateAsync(MessageDTO message, CancellationToken cancellationToken = default);
        //Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //Task<List<MessageDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        //Task<MessageDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<MessageDTO>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken = default);
    }
}
