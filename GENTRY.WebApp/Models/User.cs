using GENTRY.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Users")]
    public partial class User : Entity<Guid>
    {
        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        [Required]
        public string Phone { get; set; } = null!;

        [MaxLength(255)]
        [Required]
        public string Password { get; set; } = null!;

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        // Computed property for FullName
        [NotMapped]
        public string FullName 
        { 
            get => $"{FirstName} {LastName}".Trim(); 
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var names = value.Trim().Split(' ', 2);
                    FirstName = names[0];
                    LastName = names.Length > 1 ? names[1] : "";
                }
            }
        }

        [ForeignKey("AvatarFile")]
        public int? AvatarFileId { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? Height { get; set; }
        public int? Weight { get; set; }

        [MaxLength(50)]
        public string? SkinTone { get; set; }

        [MaxLength(50)]
        public string? BodyType { get; set; }

        public string? StylePreferences { get; set; }
        public string? SizePreferences { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsPremium { get; set; } = false;

        // Computed property for Role
        [NotMapped]
        public string Role => IsPremium ? "PremiumUser" : "NormalUser";

        public virtual File? AvatarFile { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Outfit> Outfits { get; set; } = new List<Outfit>();
        public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
        public virtual ICollection<AiTrainingData> AiTrainingData { get; set; } = new List<AiTrainingData>();
        public virtual ICollection<AIChatMessage> AIChatMessages { get; set; } = new List<AIChatMessage>();

    }

}

