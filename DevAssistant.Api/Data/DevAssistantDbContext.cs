using Microsoft.EntityFrameworkCore;
using DevAssistant.Api.Models;

namespace DevAssistant.Api.Data;

public class DevAssistantDbContext : DbContext
{
    public DevAssistantDbContext(DbContextOptions<DevAssistantDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.LastActiveAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Conversation configuration
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.CreatedAt });
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            // Foreign key relationship with User
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Conversations)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Message configuration
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ConversationId, e.CreatedAt });
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.Content).HasColumnType("nvarchar(max)");
            
            // Foreign key relationship with Conversation
            entity.HasOne(e => e.Conversation)
                  .WithMany(c => c.Messages)
                  .HasForeignKey(e => e.ConversationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Feedback configuration
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ConversationId, e.CreatedAt });
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            // Foreign key relationship with Conversation
            entity.HasOne(e => e.Conversation)
                  .WithMany(c => c.Feedbacks)
                  .HasForeignKey(e => e.ConversationId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Optional foreign key relationship with Message
            entity.HasOne(e => e.Message)
                  .WithMany()
                  .HasForeignKey(e => e.MessageId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "developer",
                Email = "developer@devassistant.local",
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow
            }
        );
    }
} 