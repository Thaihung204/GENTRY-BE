using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("OutfitItems")]
    public partial class OutfitItem : Entity<int>
    {
        [ForeignKey("Outfit")]
        public Guid OutfitId { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [MaxLength(50)]
        public string? ItemType { get; set; }

        public int PositionOrder { get; set; } = 0;

        public virtual Outfit Outfit { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }

}
