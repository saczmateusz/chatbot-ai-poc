using Microsoft.AspNetCore.Mvc;
using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.DAL.DTOs.Chat;

namespace ChatbotAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(ILogger<ChatController> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetChats(CancellationToken cancellationToken = default)
        {
            var result = await _chatService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDTO chat, CancellationToken cancellationToken = default)
        {
            if (chat == null || string.IsNullOrWhiteSpace(chat.Name))
            {
                return BadRequest("Chat name is required.");
            }
            var result = await _chatService.CreateAsync(chat, cancellationToken);
            return CreatedAtAction(nameof(CreateChat), result);
        }
    }
}
