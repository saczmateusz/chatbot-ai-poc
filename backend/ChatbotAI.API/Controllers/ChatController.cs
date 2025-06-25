using ChatbotAI.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChatbotAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;

        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Chat/GetAll")]
        public async Task<IActionResult> GetChats(CancellationToken cancellationToken = default)
        {
            // Simulate fetching chats
            var chats = new List<Chat>
            {
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Name = "Chat 1",
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Name = "Chat 2",
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    IsDeleted = false
                }
            };
            return Ok(chats);
        }

        [HttpGet("Chat/{id}")]
        public async Task<IActionResult> GetChatById(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid chat ID.");
            }

            // Simulate fetching chat by ID
            var chat = new Chat
            {
                Id = id,
                Name = "Sample Chat",
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                IsDeleted = false
            };
            return Ok(chat);
        }
    }
}
