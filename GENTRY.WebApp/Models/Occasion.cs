using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Occasions")]
    public class Occasion : Entity<int>
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }
        public virtual ICollection<Outfit> Outfits { get; set; } = new List<Outfit>();
    }
}
