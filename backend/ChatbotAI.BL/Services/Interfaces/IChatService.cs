using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.DTOs;

namespace ChatbotAI.BL.Services.Interfaces
{
    public interface IChatService
    {
        
        Task CreateAsync(ChatDTO chat, CancellationToken cancellationToken = default);
        //Task UpdateAsync(ChatDTO chat);
        //Task DeleteByIdAsync(Guid id);
        Task<List<ChatDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ChatDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
