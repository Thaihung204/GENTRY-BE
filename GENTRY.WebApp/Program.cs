using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Services.Services;
using GENTRY.WebApp.Services;
using RestX.WebApp.Services;
using GENTRY.WebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Add services -------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------- Database Context -------------------
builder.Services.AddDbContext<GENTRYDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// ------------------- Repository và Services -------------------
builder.Services.AddScoped<IRepository, EntityFrameworkRepository<GENTRYDbContext>>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IFileService, CloudinaryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IOutfitAIService, OutfitAIService>();
builder.Services.AddScoped<IExceptionHandler, GENTRY.WebApp.Services.ExceptionHandler>();
builder.Services.AddHttpContextAccessor(); // Để inject vào BaseService

// ------------------- AutoMapper -------------------
builder.Services.AddAutoMapper(typeof(Program));

// ------------------- CORS -------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // FE React
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // cho phép gửi cookie
    });
});

// ------------------- COOKIE AUTH -------------------
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/account/login";
//        options.LogoutPath = "/account/logout";
//        options.ExpireTimeSpan = TimeSpan.FromHours(1);
//        options.Cookie.HttpOnly = true;              // FE không đọc cookie được

//        // ⚡ FIX: SameSite và Secure policy phải phù hợp với nhau
//        if (builder.Environment.IsDevelopment())
//        {
//            // Development: cho phép HTTP, dùng SameSite.Lax thay vì None
//            options.Cookie.SameSite = SameSiteMode.Lax; 
//            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
//        }
//        else
//        {
//            // Production: bắt buộc HTTPS, có thể dùng SameSite.None
//            options.Cookie.SameSite = SameSiteMode.None; 
//            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//        }

//        options.ExpireTimeSpan = TimeSpan.FromHours(1);
//        options.SlidingExpiration = true; 

//        // ⚡ Trả JSON thay vì redirect khi chưa login hoặc bị cấm quyền
//        options.Events = new CookieAuthenticationEvents
//        {
//            OnRedirectToLogin = context =>
//            {
//                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                return Task.CompletedTask;
//            },
//            OnRedirectToAccessDenied = context =>
//            {
//                context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                return Task.CompletedTask;
//            }
//        };
//    });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.Cookie.HttpOnly = true;

        if (builder.Environment.IsDevelopment())
        {
            // Phải để SameSite=None mới gửi cookie qua cổng khác (localhost:3000 -> 5001)
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // bắt buộc HTTPS
        }
        else
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        }

        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseGENTRYContext(); // Phải đặt sau UseAuthentication để có thể đọc claims
app.UseAuthorization();

app.MapControllers();

app.Run();
