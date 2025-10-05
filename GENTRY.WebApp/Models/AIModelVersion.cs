using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{

    [Table("AIModelVersions")]
    public partial class AIModelVersion : Entity<int>
    {
        [Required, MaxLength(100)]
        public string ModelName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Version { get; set; } = null!;

        public string? Description { get; set; }
        public bool IsActive { get; set; } = false;
        public string? PerformanceMetrics { get; set; } 
    }
}
