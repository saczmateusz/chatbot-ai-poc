using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.DAL.DTOs.Message;

namespace ChatbotAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMediator _mediator;
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger, IMediator mediator, IMessageService messageService)
        {
            _messageService = messageService;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("AskAI")]
        public async Task<IActionResult> SendMessage([FromBody] MessageUserRequestDTO command)
        {
            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");
            Response.Headers.Append("Access-Control-Allow-Origin", "*");

            // Save user message to the database
            var userMessage = await _messageService.CreateAsync(command, HttpContext.RequestAborted);

            // Generate AI response stream
            var responseStream = await _mediator.Send(command);
            var messageId = Guid.Empty;
            var sentContent = string.Empty;

            // Stream the AI response back to the client
            await foreach (var chunk in responseStream)
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                {
                    break;
                }
                if (messageId == Guid.Empty)
                {
                    messageId = chunk.Id;
                }
                var json = JsonSerializer.Serialize(chunk);
                sentContent += chunk.Content;
                await Response.WriteAsync($"data: {json}\n\n");
                await Response.Body.FlushAsync();
            }
            // Save AI message to the database
            var aiMessage = await _messageService.CreateAIMessageAsync(sentContent, command.ChatId, messageId);
            return new EmptyResult();
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetMessagesByChatId([FromQuery] Guid chatId, CancellationToken cancellationToken = default)
        {
            if (chatId == Guid.Empty)
            {
                return BadRequest("Invalid chat ID.");
            }

            var result = await _messageService.GetByChatIdAsync(chatId, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateMessage([FromBody] MessageDTO message, CancellationToken cancellationToken = default)
        {
            if (message == null || message.Id == Guid.Empty)
            {
                return BadRequest("Invalid message data.");
            }
            await _messageService.UpdateAsync(message, cancellationToken);
            return NoContent();
        }
    }
}
