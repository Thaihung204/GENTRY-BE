using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Styles")]
    public partial class Style : Entity<int>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }

        [ForeignKey("ImageFile")]
        public int? ImageFileId { get; set; }

        public string? Tags { get; set; } 
        public bool IsActive { get; set; } = true;

        public virtual File? ImageFile { get; set; }
        public virtual ICollection<Outfit> Outfits { get; set; } = new List<Outfit>();
    }

}
