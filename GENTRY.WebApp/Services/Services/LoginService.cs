using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.AuthDTOs;
using System.Security.Claims;

namespace GENTRY.WebApp.Services.Services
{
    public class LoginService : BaseService, ILoginService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public LoginService(IRepository repo, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(repo, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        /// <summary>
        /// Đăng ký người dùng mới
        /// </summary>
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // 1. Kiểm tra email đã tồn tại chưa
                var existingUser = await Repo.GetOneAsync<User>(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Email đã được sử dụng",
                        Email = request.Email,
                        Role = ""
                    };
                }

                // 2. Tạo user mới
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    Password = request.Password, 
                    FullName = request.FullName,
                    Phone = "", 
                    IsPremium = false, 
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await Repo.CreateAsync(user);
                await Repo.SaveAsync();

                return new RegisterResponse
                {
                    Success = true,
                    Message = "Đăng ký thành công",
                    Email = user.Email,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    Email = request.Email,
                    Role = ""
                };
            }
        }

        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // 1. Tìm user theo email và password (plain text)
                var user = await Repo.GetOneAsync<User>(u => 
                    u.Email == request.Email && 
                    u.Password == request.Password &&
                    u.IsActive == true);

                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email hoặc mật khẩu không đúng",
                        Email = "",
                        FullName = "",
                        Role = ""
                    };
                }

                return new LoginResponse
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    Email = "",
                    FullName = "",
                    Role = ""
                };
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng hiện tại từ claims
        /// </summary>
        public async Task<User?> GetCurrentUserAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor?.HttpContext;
                if (httpContext?.User?.Identity?.IsAuthenticated != true)
                    return null;

                var userIdClaim = httpContext.User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return null;

                var user = await Repo.GetByIdAsync<User>(userId);
                return user?.IsActive == true ? user : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            try
            {
                return await Repo.GetExistsAsync<User>(u => u.Email == email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng theo email
        /// </summary>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await Repo.GetOneAsync<User>(u => u.Email == email && u.IsActive == true);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reset password cho người dùng
        /// </summary>
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                // 1. Kiểm tra email có tồn tại không
                var user = await Repo.GetOneAsync<User>(u => u.Email == request.Email && u.IsActive == true);
                if (user == null)
                {
                    return new ResetPasswordResponse
                    {
                        Success = false,
                        Message = "Email không tồn tại trong hệ thống"
                    };
                }

                // 2. Cập nhật password mới
                user.Password = request.NewPassword;
                
                await _userService.UpdateUserAsync(user);

                return new ResetPasswordResponse
                {
                    Success = true,
                    Message = "Đặt lại mật khẩu thành công"
                };
            }
            catch (Exception ex)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                };
            }
        }
    }
}
