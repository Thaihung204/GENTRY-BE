using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.AuthDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface ILoginService
    {
        /// <summary>
        /// Đăng ký người dùng mới
        /// </summary>
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Lấy thông tin người dùng hiện tại
        /// </summary>
        Task<User?> GetCurrentUserAsync();

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        Task<bool> IsEmailExistsAsync(string email);

        /// <summary>
        /// Lấy thông tin người dùng theo email
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Reset password
        /// </summary>
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);

        /// <summary>
        /// Đăng nhập admin
        /// </summary>
        Task<LoginResponse> AdminLoginAsync(LoginRequest request);

        /// <summary>
        /// Lấy thông tin admin hiện tại
        /// </summary>
        Task<Admin?> GetCurrentAdminAsync();

    }
}
