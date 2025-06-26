using Microsoft.EntityFrameworkCore;
using ChatbotAI.Core;
using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.Repositories.Interfaces;

namespace ChatbotAI.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        protected readonly ChatbotAIContext _context;
        protected readonly DbSet<Message> _entities;

        public MessageRepository(ChatbotAIContext context)
        {
            _context = context;
            _entities = context.Set<Message>();
        }
        public void Create(Message entity)
        {
            _entities.Add(entity);
        }

        public void Update(Message entity)
        {
            _entities.Update(entity);
        }

        public void Delete(Message entity)
        {
            entity.IsDeleted = true;
            _entities.Update(entity);
        }

        public void DeleteById(Guid id)
        {
            var entity = _entities.Find(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _entities.Update(entity);
            }
        }

        public async Task<List<Message>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _entities.ToListAsync(cancellationToken);
            return result;
        }

        public async Task<List<Message>> GetByChatIdAsync(Guid chatId, CancellationToken cancellationToken = default)
        {
            var result = await _entities
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.DateCreated).ToListAsync(cancellationToken);
            return result;
        }

        public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _entities.FindAsync(new object[] { id }, cancellationToken);
            return result;
        }
    }
}
