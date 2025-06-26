using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.DTOs.Chat;
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

        public async Task<GetChatDTO> CreateAsync(CreateChatDTO chat, CancellationToken cancellationToken = default)
        {
            var chatGuid = Guid.NewGuid();
            var entity = new Chat()
            {
                Id = chatGuid,
                Name = chat.Name,
            };

            UOW.Chats.Create(entity);
            await UOW.SaveAsync(cancellationToken);
            return new GetChatDTO()
            {
                Id = entity.Id,
                DateCreated = entity.DateCreated,
                Name = entity.Name,
            };
        }

        public async Task<List<GetChatDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var chats = await UOW.Chats.GetAllAsync(cancellationToken);
            return chats.Select(chat => new GetChatDTO()
            {
                Id = chat.Id,
                DateCreated = chat.DateCreated,
                Name = chat.Name,
            }).ToList();
        }
    }
}
