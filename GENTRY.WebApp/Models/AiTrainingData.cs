using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("AITrainingData")]
    public partial class AiTrainingData : Entity<int>
    {
        [ForeignKey("Outfit")]
        public Guid OutfitId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [MaxLength(20)]
        public string? FeedbackType { get; set; }

        public string? ContextData { get; set; } 

        public virtual Outfit Outfit { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
