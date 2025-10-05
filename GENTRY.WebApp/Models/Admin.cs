using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Admin")]
    public partial class Admin : Entity<int>
    {
        [Required, MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Password { get; set; } = null!;

        public string? Permissions { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
