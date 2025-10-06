using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs
{
    public class AiStylingRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<int> StyleIds { get; set; } = new List<int>();
        public int? OccasionId { get; set; }
        public int? WeatherId { get; set; }
        public List<int> ColorIds { get; set; } = new List<int>();
        public decimal? MaxBudget { get; set; }
        [MaxLength(20)]
        public string? Gender { get; set; }
        [MaxLength(10)]
        public string? PreferredSize { get; set; }
        [MaxLength(500)]
        public string? AdditionalRequirements { get; set; }
        [Range(1, 5)]
        public int NumberOfSuggestions { get; set; } = 1;
    }
}


