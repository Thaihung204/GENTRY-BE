using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Weathers")]
    public class Weather : Entity<int>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!; 

        [MaxLength(255)]
        public string? Description { get; set; }
        public virtual ICollection<Outfit> Outfits { get; set; } = new List<Outfit>();

    }
}
