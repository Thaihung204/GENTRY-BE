using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("OutfitFeatures")]
    public partial class OutfitFeature : Entity<int>
    {
        [ForeignKey("Outfit")]
        public Guid OutfitId { get; set; }

        [ForeignKey("Feature")]
        public int FeatureId { get; set; }

        public virtual Outfit Outfit { get; set; } = null!;
        public virtual Feature Feature { get; set; } = null!;
        public virtual ICollection<OutfitFeatureValue> Values { get; set; } = new List<OutfitFeatureValue>();
    }
}
