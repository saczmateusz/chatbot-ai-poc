using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.DTOs;
using ChatbotAI.DAL.Services.Interfaces;

namespace ChatbotAI.BL.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork UOW;

        public ChatService(IUnitOfWork uow)
        {
            UOW = uow;
        }

        public async Task CreateAsync(ChatDTO chat, CancellationToken cancellationToken = default)
        {
            var chatGuid = Guid.NewGuid();

            var entity = new Chat()
            {
                Id = chatGuid,
                Name = chat.Name,
            };

            UOW.Chats.Create(entity);
            await UOW.SaveAsync(cancellationToken);
        }

        public async Task<List<ChatDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var chats = await UOW.Chats.GetAllAsync(cancellationToken);
            return chats.Select(chat => new ChatDTO()
            {
                Id = chat.Id,
                Name = chat.Name
            }).ToList();
        }

        public async Task<ChatDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            var chat = await UOW.Chats.GetByIdAsync(id, cancellationToken);
            if (chat == null)
            {
                return null;
            }
            return new ChatDTO()
            {
                Id = chat.Id,
                Name = chat.Name
            };
        }
    }
}
