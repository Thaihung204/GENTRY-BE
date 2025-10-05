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
                // Lấy UserId từ claims
                var userIdClaim = context.User.FindFirst("Id")?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
                {
                    gentryContext.UserId = userId;
                }

                // Lấy AdminId nếu user là admin (có thể mở rộng sau)
                var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
                if (roleClaim == "Admin" && !string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var adminId))
                {
                    gentryContext.AdminId = adminId;
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