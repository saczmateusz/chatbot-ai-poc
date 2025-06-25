using ChatbotAI.Core;
using ChatbotAI.DAL.Repositories.Interfaces;
using ChatbotAI.DAL.Services.Interfaces;

namespace ChatbotAI.DAL.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly ChatbotAIContext _context;

        public IChatRepository Chats { get; }
        public IMessageRepository Messages { get; }

        public UnitOfWork(ChatbotAIContext context, IChatRepository chats, IMessageRepository messages)
        {
            _context = context;
            Chats = chats;
            Messages = messages;
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
