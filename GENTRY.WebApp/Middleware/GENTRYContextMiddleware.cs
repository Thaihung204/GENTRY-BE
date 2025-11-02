using GENTRY.WebApp.Models;
using System.Security.Claims;

namespace GENTRY.WebApp.Middleware
{
    public class GENTRYContextMiddleware
    {
        private readonly RequestDelegate _next;

        public GENTRYContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Tạo GENTRYContext và thiết lập UserId từ claims
            var gentryContext = new GENTRYContext();

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                // Kiểm tra UserType để phân biệt User và Admin
                var userType = context.User.FindFirst("UserType")?.Value;
                
                if (userType == "Admin")
                {
                    // Xử lý cho Admin (AdminId là int)
                    var adminIdClaim = context.User.FindFirst("AdminId")?.Value ?? context.User.FindFirst("Id")?.Value;
                    if (!string.IsNullOrEmpty(adminIdClaim) && int.TryParse(adminIdClaim, out var adminId))
                    {
                        // Note: GENTRYContext.AdminId là Guid, nhưng Admin.ID là int
                        // Có thể cần cập nhật GENTRYContext để hỗ trợ int AdminId trong tương lai
                        // Hiện tại để trống hoặc convert nếu cần
                    }
                }
                else
                {
                    // Xử lý cho User (UserId là Guid)
                    var userIdClaim = context.User.FindFirst("Id")?.Value;
                    if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
                    {
                        gentryContext.UserId = userId;
                    }
                }
            }

            // Lưu vào HttpContext.Items để BaseService có thể truy cập
            context.Items["GENTRYContext"] = gentryContext;

            await _next(context);
        }
    }

    // Extension method để dễ dàng thêm vào pipeline
    public static class GENTRYContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseGENTRYContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GENTRYContextMiddleware>();
        }
    }
} 