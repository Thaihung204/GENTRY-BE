using System;
using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.ChatDTOs
{
    public class ChatDto
    {
        public Guid UserId { get; set; }

        [MaxLength(5000)]
        public string UserMessage { get; set; } = null!;

        [MaxLength(10000)]
        public string AIResponse { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsFromUser { get; set; } = true; // true = user message, false = AI response

        public string? AdditionalContext { get; set; } // JSON string for any additional data
    }
}
