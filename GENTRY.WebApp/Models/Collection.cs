using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Collections")]
    public partial class Collection : Entity<Guid>
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [ForeignKey("CoverFile")]
        public int? CoverFileId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual File? CoverFile { get; set; }
        public virtual ICollection<CollectionItem> Items { get; set; } = new List<CollectionItem>();
    }
}
