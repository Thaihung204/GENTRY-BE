using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("ChatHistory")]
    public partial class ChatHistory : Entity<Guid>
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required, MaxLength(1000)]
        public string UserMessage { get; set; } = null!;

        [Required]
        public string AiResponse { get; set; } = null!;

        [MaxLength(100)]
        public string? Occasion { get; set; }

        [MaxLength(100)]
        public string? WeatherCondition { get; set; }

        [MaxLength(50)]
        public string? Season { get; set; }

        [MaxLength(500)]
        public string? AdditionalPreferences { get; set; }

        [ForeignKey("GeneratedOutfit")]
        public Guid? GeneratedOutfitId { get; set; }

        [MaxLength(50)]
        public string ChatType { get; set; } = "OutfitRecommendation"; // OutfitRecommendation, GeneralStyling, etc.

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Outfit? GeneratedOutfit { get; set; }
    }
}
