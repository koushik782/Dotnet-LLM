using System.ComponentModel.DataAnnotations;

namespace DevAssistant.Api.Models;

public class Conversation
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? Template { get; set; }
    
    [StringLength(50)]
    public string Model { get; set; } = "mistral";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsArchived { get; set; } = false;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
} 