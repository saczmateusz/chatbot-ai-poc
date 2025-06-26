using MediatR;
using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.DAL.DTOs.Message;

namespace ChatbotAI.BL.ExternalAPIs
{
    public class MediatRChatMessageHandler : IRequestHandler<MessageUserRequestDTO, IAsyncEnumerable<MessageAIResponseDTO>>
    {
        private readonly IMessageService _messageService;

        public MediatRChatMessageHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IAsyncEnumerable<MessageAIResponseDTO>> Handle(MessageUserRequestDTO request, CancellationToken cancellationToken)
        {
            return _messageService.GenerateResponseStreamAsync(request.Message, request.ChatId, cancellationToken);
        }
    }
}
