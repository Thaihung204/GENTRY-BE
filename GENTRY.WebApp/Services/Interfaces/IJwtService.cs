using GENTRY.WebApp.Models;
using System.Security.Claims;

namespace GENTRY.WebApp.Services.Interfaces
{
    /// <summary>
    /// Service xử lý JWT tokens
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Tạo JWT access token từ thông tin user
        /// </summary>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Tạo refresh token
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Validate và parse JWT token
        /// </summary>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Lấy thông tin user từ JWT token
        /// </summary>
        Guid? GetUserIdFromToken(string token);

        /// <summary>
        /// Kiểm tra token có hợp lệ không
        /// </summary>
        bool IsTokenValid(string token);
    }
}
