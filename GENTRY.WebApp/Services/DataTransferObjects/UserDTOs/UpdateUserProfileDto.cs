using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.UserDTOs
{
    public class UpdateUserProfileDto
    {
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(50)]
        public string? FirstName { get; set; }
        
        [MaxLength(50)]
        public string? LastName { get; set; }
        
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
    }
}
