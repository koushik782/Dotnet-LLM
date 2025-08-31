using System.ComponentModel.DataAnnotations;

namespace DevAssistant.Api.Models;

public class Feedback
{
    public int Id { get; set; }
    
    public int ConversationId { get; set; }
    
    public int? MessageId { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Type { get; set; } = string.Empty; // "thumbs_up", "thumbs_down"
    
    [StringLength(1000)]
    public string? Comment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Conversation Conversation { get; set; } = null!;
    public virtual Message? Message { get; set; }
} 