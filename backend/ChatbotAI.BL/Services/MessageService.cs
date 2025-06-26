using System.Runtime.CompilerServices;
using ChatbotAI.BL.ExternalAPIs.Interfaces;
using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.Core.Domain;
using ChatbotAI.Core.Enums;
using ChatbotAI.DAL.DTOs.Message;
using ChatbotAI.DAL.Services.Interfaces;

namespace ChatbotAI.BL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork UOW;
        private readonly IAIServiceMock _aiServiceMock;

        public MessageService(IUnitOfWork uow, IAIServiceMock aIServiceMock)
        {
            UOW = uow;
            _aiServiceMock = aIServiceMock;
        }

        public async IAsyncEnumerable<MessageAIResponseDTO> GenerateResponseStreamAsync(
            string message,
            Guid chatId,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var messageId = Guid.NewGuid();
            // Simulate processing the message and generating response chunks
            var responseText = await _aiServiceMock.SimulateAIResponse(message);
            var words = responseText.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                var chunk = words[i];
                if (i < words.Length - 1) chunk += " ";

                yield return new MessageAIResponseDTO
                {
                    Id = messageId,
                    Content = chunk,
                    IsComplete = i == words.Length - 1
                };

                // Add delay to simulate real-time generation
                await Task.Delay(100, cancellationToken);
            }
        }

        public async Task<Message> CreateAsync(MessageUserRequestDTO message, CancellationToken cancellationToken = default)
        {
            var messageGuid = Guid.NewGuid();
            var entity = new Message()
            {
                Id = messageGuid,
                ChatId = message.ChatId,
                Content = message.Message,
                Sender = "user",
                Reaction = ReactionType.None
            };
            UOW.Messages.Create(entity);
            await UOW.SaveAsync(cancellationToken);
            return entity;
        }

        public async Task<Message> CreateAIMessageAsync(string content, Guid chatId, Guid messageId, CancellationToken cancellationToken = default)
        {
            var entity = new Message()
            {
                Id = messageId,
                ChatId = chatId,
                Content = content,
                Sender = "ai",
                Reaction = ReactionType.None
            };
            UOW.Messages.Create(entity);
            await UOW.SaveAsync(cancellationToken);
            return entity;
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
            // entity.Content = message.Content; // Currently unchangeable
            entity.Reaction = message.Reaction;
            UOW.Messages.Update(entity);
            await UOW.SaveAsync(cancellationToken);
        }

        public async Task<List<MessageDTO>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken = default)
        {
            var messages = await UOW.Messages.GetByChatIdAsync(chatId, cancellationToken);
            return messages
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
