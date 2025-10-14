using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Services.Services;
using GENTRY.WebApp.Services;
using RestX.WebApp.Services;
using GENTRY.WebApp.Middleware;
using System.Text.Json;

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
builder.Services.AddScoped<IStyleService, StyleService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IOccasionService, OccasionService>();
builder.Services.AddScoped<IGeminiAIService, GeminiAIService>();
builder.Services.AddScoped<IChatHistoryService, ChatHistoryService>();
builder.Services.AddScoped<IExceptionHandler, GENTRY.WebApp.Services.ExceptionHandler>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpContextAccessor(); // Để inject vào BaseService

// ------------------- AutoMapper -------------------
builder.Services.AddAutoMapper(typeof(Program));

// External HttpClients
builder.Services.AddHttpClient<GeminiAIService>();

// ------------------- CORS -------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000", "https://gentry.vercel.app") // chỉ rõ domain
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// ------------------- JWT AUTH -------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentNullException("JWT SecretKey is required"))),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    Success = false,
                    Message = "Token không hợp lệ",
                    Error = context.Exception.Message
                });

                return context.Response.WriteAsync(result);
            }

            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    Success = false,
                    Message = "Bạn cần đăng nhập để truy cập tài nguyên này"
                });

                return context.Response.WriteAsync(result);
            }

            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    Success = false,
                    Message = "Bạn không có quyền truy cập tài nguyên này"
                });

                return context.Response.WriteAsync(result);
            }

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
