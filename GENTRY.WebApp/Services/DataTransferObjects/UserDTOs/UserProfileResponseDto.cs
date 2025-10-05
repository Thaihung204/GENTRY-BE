namespace GENTRY.WebApp.Services.DataTransferObjects.UserDTOs
{
    public class UserProfileResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserProfileDto? Data { get; set; }
    }
} 