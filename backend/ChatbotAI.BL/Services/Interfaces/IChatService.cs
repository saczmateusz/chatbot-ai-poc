using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.DTOs.Chat;

namespace ChatbotAI.BL.Services.Interfaces
{
    public interface IChatService
    {
        Task<GetChatDTO> CreateAsync(CreateChatDTO chat, CancellationToken cancellationToken = default);
        Task<List<GetChatDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
