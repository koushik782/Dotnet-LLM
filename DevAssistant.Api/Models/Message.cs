using System.ComponentModel.DataAnnotations;

namespace DevAssistant.Api.Models;

public class Message
{
    public int Id { get; set; }
    
    public int ConversationId { get; set; }
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Role { get; set; } = string.Empty; // "user" or "assistant"
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int? TokenCount { get; set; }
    
    public TimeSpan? ResponseTime { get; set; }
    
    // Navigation properties
    public virtual Conversation Conversation { get; set; } = null!;
} 