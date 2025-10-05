using GENTRY.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Files")]
    public partial class File : Entity<int>
    {
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = null!;

        [Required, MaxLength(500)]
        public string Url { get; set; } = null!;

        [ForeignKey("UploadedByUser")]
        public Guid? UploadedBy { get; set; }

        public virtual User? UploadedByUser { get; set; }
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Style> Styles { get; set; } = new List<Style>();
    }
}
