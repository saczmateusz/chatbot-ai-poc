using ChatbotAI.Core.Domain;

namespace ChatbotAI.DAL.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public void Create(Chat entity);
        public void Update(Chat entity);
        public void Delete(Chat entity);
        public void DeleteById(Guid id);
        public Task<List<Chat>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
