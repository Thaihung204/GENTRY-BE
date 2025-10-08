using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Services.DataTransferObjects.AuthDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public AuthController(IExceptionHandler exceptionHandler, ILoginService loginService) 
            : base(exceptionHandler)
        {
            _loginService = loginService;
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

                // 3. Tìm user để lấy thông tin đầy đủ cho claims
                var user = await _loginService.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return Unauthorized(new LoginResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy thông tin người dùng",
                        Email = "",
                        FullName = "",
                        Role = ""
                    });
                }

                // 4. Tạo Claims cho cookie session
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("IsPremium", user.IsPremium.ToString())
                };

                // 5. Tạo ClaimsIdentity và ClaimsPrincipal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // 6. Đăng nhập (tạo cookie session)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = false, // Session cookie (xóa khi đóng browser)
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // 1 giờ như config
                    });

                // 7. Trả kết quả thành công
                return Ok(new { 
                    success = true,
                    data = result
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
        /// Đăng xuất người dùng
        /// POST: api/auth/logout
        /// </summary>
        [HttpPost("logout")]
        [Authorize] // Yêu cầu đã đăng nhập
        public async Task<IActionResult> Logout()
        {
            try
            {
                // 1. Xóa cookie session
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // 2. Trả kết quả
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
