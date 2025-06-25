using ChatbotAI.Core;
using ChatbotAI.Core.Domain;
using ChatbotAI.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.DAL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        protected readonly ChatbotAIContext _context;
        protected readonly DbSet<Chat> _entities;

        public ChatRepository(ChatbotAIContext context)
        {
            _context = context;
            _entities = context.Set<Chat>();
        }
        public void Create(Chat entity)
        {
            _entities.Add(entity);
        }

        public void Update(Chat entity)
        {
            _entities.Update(entity);
        }

        public void Delete(Chat entity)
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

        public async Task<List<Chat>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _entities.ToListAsync(cancellationToken);
            return result;
        }

        public async Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _entities.FindAsync(new object[] { id }, cancellationToken);
            return result;
        }
    }
}
