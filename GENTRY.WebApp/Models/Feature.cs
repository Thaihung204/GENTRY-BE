using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Features")]
    public partial class Feature : Entity<int>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? FeatureType { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<OutfitFeature> OutfitFeatures { get; set; } = new List<OutfitFeature>();
    }
}
