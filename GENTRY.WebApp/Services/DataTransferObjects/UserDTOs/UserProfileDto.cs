namespace GENTRY.WebApp.Services.DataTransferObjects.UserDTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int? AvatarFileId { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string? SkinTone { get; set; }
        public string? BodyType { get; set; }
        public string? StylePreferences { get; set; }
        public string? SizePreferences { get; set; }
        public bool IsActive { get; set; }
        public bool IsPremium { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
