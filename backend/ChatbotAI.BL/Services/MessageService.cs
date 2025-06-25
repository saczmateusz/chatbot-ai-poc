using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.Core.Enums;
using ChatbotAI.DAL.DTOs;
using ChatbotAI.DAL.Services.Interfaces;

namespace ChatbotAI.BL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork UOW;

        public MessageService(IUnitOfWork uow)
        {
            UOW = uow;
        }

        public async Task CreateAsync(MessageDTO message, CancellationToken cancellationToken = default)
        {
            var messageGuid = Guid.NewGuid();
            var entity = new Core.Domain.Message()
            {
                Id = messageGuid,
                ChatId = message.ChatId,
                Content = message.Content,
                Sender = message.Sender,
                Reaction = ReactionType.None
            };
            UOW.Messages.Create(entity);
            await UOW.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(MessageDTO message, CancellationToken cancellationToken = default)
        {
            if (message.Id == Guid.Empty)
            {
                return;
            }
            var entity = await UOW.Messages.GetByIdAsync(message.Id, cancellationToken);
            if (entity == null)
            {
                return;
            }
            entity.Content = message.Content;
            entity.Sender = message.Sender;
            entity.Reaction = message.Reaction;
            UOW.Messages.Update(entity);
            await UOW.SaveAsync(cancellationToken);
        }

        public async Task<List<MessageDTO>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken = default)
        {
            var messages = await UOW.Messages.GetAllAsync(cancellationToken); // TODO: optimize this to fetch only messages for the specific chatId
            return messages
                .Where(m => m.ChatId == chatId && !m.IsDeleted)
                .Select(m => new MessageDTO()
                {
                    Id = m.Id,
                    ChatId = m.ChatId,
                    Content = m.Content,
                    Sender = m.Sender,
                    Reaction = m.Reaction
                }).ToList();
        }
    }
}
