using GENTRY.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Items")]
    public partial class Item : Entity<Guid>
    {
        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? Brand { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("File")]
        public int? FileId { get; set; }

        [MaxLength(500)]
        public string? SourceUrl { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Color")]
        public int? ColorId { get; set; }

        [MaxLength(20)]
        public string? Size { get; set; }

        [MaxLength(500)]
        public string? Tags { get; set; }

        public decimal? Price { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public virtual User? User { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual File? File { get; set; }
        public virtual Color? Color { get; set; }


        public virtual ICollection<OutfitItem> OutfitItems { get; set; } = new List<OutfitItem>();
        public virtual ICollection<AffiliateLink> AffiliateLinks { get; set; } = new List<AffiliateLink>();
    }
}
