using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Outfits")]
    public partial class Outfit : Entity<Guid>
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        [ForeignKey("Style")]
        public int? StyleId { get; set; }

        public string? Description { get; set; }
        public string? Occasion { get; set; }
        public string? WeatherCondition { get; set; }
        public string? Season { get; set; }
        public string? BodyType { get; set; }

        public bool IsAiGenerated { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public int LikesCount { get; set; } = 0;
        public int ViewsCount { get; set; } = 0;

        public virtual User User { get; set; } = null!;
        public virtual Style? Style { get; set; }
        public virtual ICollection<OutfitItem> OutfitItems { get; set; } = new List<OutfitItem>();
        public virtual ICollection<OutfitFeature> OutfitFeatures { get; set; } = new List<OutfitFeature>();
        public virtual ICollection<AiTrainingData> AiTrainingData { get; set; } = new List<AiTrainingData>();

    }
}
