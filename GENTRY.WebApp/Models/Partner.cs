using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Partners")]
    public partial class Partner : Entity<Guid>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? DomainUrl { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [ForeignKey("LogoFile")]
        public int? LogoFileId { get; set; }

        public decimal CommissionRate { get; set; } = 0;
        public string? ApiEndpoint { get; set; }
        public string? ApiKey { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual File? LogoFile { get; set; }
        public virtual ICollection<AffiliateLink> AffiliateLinks { get; set; } = new List<AffiliateLink>();
    }

}
