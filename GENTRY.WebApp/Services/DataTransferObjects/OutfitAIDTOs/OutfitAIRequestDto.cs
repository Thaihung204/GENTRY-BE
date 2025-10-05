using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs
{
    public class OutfitAIRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required, MaxLength(1000)]
        public string UserMessage { get; set; } = null!;

        [MaxLength(100)]
        public string? Occasion { get; set; }

        [MaxLength(50)]
        public string? WeatherCondition { get; set; }

        [MaxLength(20)]
        public string? Season { get; set; }

        [MaxLength(500)]
        public string? AdditionalPreferences { get; set; }
    }
} 