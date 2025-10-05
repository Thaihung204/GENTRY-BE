using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Analysis")]
    public partial class Analysis : Entity<int>
    {
        [Required, MaxLength(100)]
        public string MetricName { get; set; } = null!;

        public decimal? MetricValue { get; set; }
        public string? MetricData { get; set; } 
    }
}
