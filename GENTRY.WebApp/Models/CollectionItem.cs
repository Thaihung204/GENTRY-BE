using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("CollectionItems")]
    public partial class CollectionItem : Entity<int>
    {
        [ForeignKey("Collection")]
        public Guid CollectionId { get; set; }

        [ForeignKey("Outfit")]
        public Guid OutfitId { get; set; }

        public virtual Collection Collection { get; set; } = null!;
        public virtual Outfit Outfit { get; set; } = null!;
    }
}
