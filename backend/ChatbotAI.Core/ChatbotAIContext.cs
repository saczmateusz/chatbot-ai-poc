using ChatbotAI.Core.Interfaces;
using ChatbotAI.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.Core
{
    public class ChatbotAIContext : DbContext
    {
        private const string _keyIsDeleted = "IsDeleted";

        public ChatbotAIContext(DbContextOptions<ChatbotAIContext> options)
            : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Chat>().Property<bool>(_keyIsDeleted);
            builder.Entity<Chat>().HasQueryFilter(c => EF.Property<bool>(c, _keyIsDeleted) == false);

            builder.Entity<Message>().Property<bool>(_keyIsDeleted);
            builder.Entity<Message>().HasQueryFilter(m => EF.Property<bool>(m, _keyIsDeleted) == false);
            builder.Entity<Message>().Property(x => x.Reaction).HasConversion<string>();
            builder.Entity<Message>()
                .HasOne(x => x.Chat)
                .WithMany(y => y.Messages)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateCoreProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateCoreProperties()
        {
            var allTrackedEntities = ChangeTracker.Entries().ToList();
            var trackedEntities = allTrackedEntities.Where(x => x.State == EntityState.Added ||
                                                                x.State == EntityState.Modified ||
                                                                x.State == EntityState.Deleted)
                                                    .ToList();
            foreach (var entry in trackedEntities)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[nameof(IDomainEntity.IsDeleted)] = false;
                        entry.CurrentValues[nameof(IDomainEntity.DateCreated)] = DateTime.UtcNow;
                        entry.CurrentValues[nameof(IDomainEntity.DateModified)] = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.CurrentValues[nameof(IDomainEntity.DateModified)] = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
