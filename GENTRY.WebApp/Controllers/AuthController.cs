using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Services.DataTransferObjects.AuthDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly IJwtService _jwtService;
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthController(IExceptionHandler exceptionHandler, ILoginService loginService, IJwtService jwtService, IRepository repository, IConfiguration configuration) 
            : base(exceptionHandler)
        {
            _loginService = loginService;
            _jwtService = jwtService;
            _repository = repository;
            _configuration = configuration;
        }

        /// <summary>
        /// Đăng ký người dùng mới
        /// POST: api/auth/register
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // 1. Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RegisterResponse
                    {
                        Success = false,
                        Message = "Dữ liệu không hợp lệ",
                        Email = request?.Email ?? "",
                        Role = ""
                    });
                }

                // 2. Gọi service để đăng ký
                var result = await _loginService.RegisterAsync(request);

                // 3. Trả kết quả
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RegisterResponse
                {
                    Success = false,
                    Message = "Lỗi máy chủ nội bộ",
                    Email = request?.Email ?? "",
                    Role = ""
                });
            }
        }

        /// <summary>
        /// Đăng nhập người dùng
        /// POST: api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // 1. Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(new LoginResponse
                    {
                        Success = false,
                        Message = "Email và mật khẩu là bắt buộc",
                        Email = "",
                        FullName = "",
                        Role = ""
                    });
                }

                // 2. Gọi service để xác thực
                var result = await _loginService.LoginAsync(request);

                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                // 3. Trả kết quả thành công (JWT đã được tạo trong LoginService)
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse
                {
                    Success = false,
                    Message = "Lỗi máy chủ nội bộ",
                    Email = "",
                    FullName = "",
                    Role = ""
                });
            }
        }

        /// <summary>
        /// Đăng xuất người dùng
        /// POST: api/auth/logout
        /// </summary>
        [HttpPost("logout")]
        [Authorize] // Yêu cầu đã đăng nhập
        public IActionResult Logout()
        {
            try
            {
                // Với JWT, logout chỉ cần client xóa token
                // Server không cần làm gì đặc biệt
                // Nếu muốn blacklist token, có thể implement thêm
                
                return Ok(new { Success = true, Message = "Đăng xuất thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Lỗi máy chủ nội bộ" });
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng hiện tại
        /// GET: api/auth/current-user
        /// </summary>
        [HttpGet("current-user")]
        [Authorize] // Yêu cầu đã đăng nhập
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                // 1. Lấy thông tin user từ service
                var user = await _loginService.GetCurrentUserAsync();

                if (user == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại" });
                }

                // 2. Trả thông tin user
                return Ok(new
                {
                    Success = true,
                    Data = new
                    {
                        user.Email,
                        user.FullName,
                        user.Role,
                        user.IsPremium,
                        user.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Lỗi máy chủ nội bộ" });
            }
        }

        /// <summary>
        /// Reset password cho người dùng
        /// POST: api/auth/reset-password
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                // 1. Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResetPasswordResponse
                    {
                        Success = false,
                        Message = "Dữ liệu không hợp lệ"
                    });
                }

                // 2. Gọi service để reset password
                var result = await _loginService.ResetPasswordAsync(request);

                // 3. Trả kết quả
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Lỗi máy chủ nội bộ"
                });
            }
        }

        /// <summary>
        /// Làm mới JWT token
        /// POST: api/auth/refresh-token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // 1. Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(new LoginResponse
                    {
                        Success = false,
                        Message = "Dữ liệu không hợp lệ",
                        Email = "",
                        FullName = "",
                        Role = ""
                    });
                }

                // 2. Validate access token (có thể expired)
                var userId = _jwtService.GetUserIdFromToken(request.AccessToken);
                if (userId == null)
                {
                    return Unauthorized(new LoginResponse
                    {
                        Success = false,
                        Message = "Access token không hợp lệ",
                        Email = "",
                        FullName = "",
                        Role = ""
                    });
                }

                // 3. Lấy thông tin user bằng ID
                var user = await _repository.GetByIdAsync<GENTRY.WebApp.Models.User>(userId.Value);
                
                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new LoginResponse
                    {
                        Success = false,
                        Message = "Người dùng không tồn tại hoặc đã bị khóa",
                        Email = "",
                        FullName = "",
                        Role = ""
                    });
                }

                // 4. Tạo token mới
                var newAccessToken = _jwtService.GenerateAccessToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                var expiryInHours = int.Parse(_configuration["JWT:ExpiryInHours"] ?? "24");
                var tokenExpiry = DateTime.UtcNow.AddHours(expiryInHours);

                return Ok(new LoginResponse
                {
                    Success = true,
                    Message = "Làm mới token thành công",
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    TokenExpiry = tokenExpiry,
                    IsPremium = user.IsPremium,
                    IsActive = user.IsActive
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse
                {
                    Success = false,
                    Message = "Lỗi máy chủ nội bộ",
                    Email = "",
                    FullName = "",
                    Role = ""
                });
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái đăng nhập
        /// GET: api/auth/check
        /// </summary>
        [HttpGet("check")]
        public IActionResult CheckAuthStatus()
        {
            try
            {
                var isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
                
                if (isAuthenticated)
                {
                    var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                    var fullName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                    var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                    return Ok(new
                    {
                        Success = true,
                        IsAuthenticated = true,
                        User = new
                        {
                            Email = email,
                            FullName = fullName,
                            Role = role
                        }
                    });
                }

                return Ok(new
                {
                    Success = true,
                    IsAuthenticated = false,
                    User = (object?)null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Lỗi máy chủ nội bộ" });
            }
        }
    }
}
