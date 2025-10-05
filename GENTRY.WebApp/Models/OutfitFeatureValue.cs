using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("OutfitFeatureValues")]
    public partial class OutfitFeatureValue : Entity<int>
    {
        [ForeignKey("OutfitFeature")]
        public int OutfitFeatureId { get; set; }

        [Required, MaxLength(255)]
        public string Value { get; set; } = null!;

        public virtual OutfitFeature OutfitFeature { get; set; } = null!;
    }
}
