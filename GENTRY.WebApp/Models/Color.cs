using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Colors")]
    public partial class Color : Entity<int>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(10)]
        public string? HexCode { get; set; }

        [MaxLength(20)]
        public string? RgbValues { get; set; }

        [MaxLength(50)]
        public string? ColorFamily { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
