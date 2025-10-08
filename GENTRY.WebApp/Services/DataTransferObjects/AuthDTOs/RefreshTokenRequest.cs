using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.AuthDTOs
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Access token là bắt buộc")]
        public string AccessToken { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Refresh token là bắt buộc")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
