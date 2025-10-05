using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.UserDTOs
{
    public class UpdateUserRequest
    {
        [MaxLength(255), EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? IsPremium { get; set; }
    }
}
