using ChatbotAI.Core.Domain;

namespace ChatbotAI.DAL.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        public void Create(Message entity);
        public void Update(Message entity);
        public void Delete(Message entity);
        public void DeleteById(Guid id);
        public Task<List<Message>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
