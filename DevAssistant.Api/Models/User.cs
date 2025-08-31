using System.ComponentModel.DataAnnotations;

namespace DevAssistant.Api.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Email { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
} 