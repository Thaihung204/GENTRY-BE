using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("AffiliateLinks")]
    public partial class AffiliateLink : Entity<int>
    {
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [ForeignKey("Partner")]
        public Guid PartnerId { get; set; }

        [Required, MaxLength(500)]
        public string Url { get; set; } = null!;

        public decimal? Price { get; set; }
        [MaxLength(10)]
        public string Currency { get; set; } = "VND";
        public bool IsActive { get; set; } = true;

        public virtual Item Item { get; set; } = null!;
        public virtual Partner Partner { get; set; } = null!;
    }
}
