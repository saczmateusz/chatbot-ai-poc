using ChatbotAI.DAL.Repositories.Interfaces;

namespace ChatbotAI.DAL.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IChatRepository Chats { get; }
        public IMessageRepository Messages { get; }

        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
