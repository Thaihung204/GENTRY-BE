using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs
{
    public class OutfitAIRequestDto
    {
        [Required, MaxLength(1000)]
        public string UserMessage { get; set; } = null!;
    }
} 