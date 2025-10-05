using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("SizeCharts")]
    public partial class SizeChart : Entity<int>
    {
        [Required, MaxLength(100)]
        public string Brand { get; set; } = null!;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string? SizeData { get; set; } 

        public virtual Category Category { get; set; } = null!;
    }

}
