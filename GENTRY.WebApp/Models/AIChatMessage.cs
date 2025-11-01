using GENTRY.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("AIChatMessages")]
    public class AIChatMessage : Entity<Guid>
    {
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required, MaxLength(5000)]
        public string UserMessage { get; set; } = null!;

        [Required, MaxLength(10000)]
        public string AIResponse { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsFromUser { get; set; } = true; // true = user message, false = AI response

        public string? AdditionalContext { get; set; } // JSON string for any additional data

        // Navigation property
        public virtual User? User { get; set; } 
    }
}


