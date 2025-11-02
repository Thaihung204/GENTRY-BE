using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GENTRY.WebApp.Services.Services
{
    /// <summary>
    /// Service xử lý JWT tokens
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryInHours;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["JWT:SecretKey"] ?? throw new ArgumentNullException("JWT SecretKey is required");
            _issuer = _configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT Issuer is required");
            _audience = _configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT Audience is required");
            _expiryInHours = int.Parse(_configuration["JWT:ExpiryInHours"] ?? "24");
        }

        /// <summary>
        /// Tạo JWT access token từ thông tin user
        /// </summary>
        public string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("IsPremium", user.IsPremium.ToString()),
                new Claim("UserType", "User"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, 
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                    ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_expiryInHours),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Tạo JWT access token từ thông tin admin
        /// </summary>
        public string GenerateAdminAccessToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new Claim("Id", admin.Id.ToString()),
                new Claim("AdminId", admin.Id.ToString()),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("UserType", "Admin"),
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, 
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                    ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_expiryInHours),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Tạo refresh token
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Validate và parse JWT token
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var isValidAlgorithm = jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!isValidAlgorithm)
                        return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy thông tin user ID từ JWT token
        /// </summary>
        public Guid? GetUserIdFromToken(string token)
        {
            try
            {
                var principal = ValidateToken(token);
                if (principal == null)
                    return null;

                // Kiểm tra xem đây có phải là admin token không
                var userType = principal.FindFirst("UserType")?.Value;
                if (userType == "Admin")
                    return null; // Admin không có Guid ID

                var userIdClaim = principal.FindFirst("Id")?.Value ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                
                if (string.IsNullOrEmpty(userIdClaim))
                    return null;

                if (Guid.TryParse(userIdClaim, out var userId))
                    return userId;

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy thông tin admin ID từ JWT token
        /// </summary>
        public int? GetAdminIdFromToken(string token)
        {
            try
            {
                var principal = ValidateToken(token);
                if (principal == null)
                    return null;

                // Kiểm tra xem đây có phải là admin token không
                var userType = principal.FindFirst("UserType")?.Value;
                if (userType != "Admin")
                    return null; // Không phải admin token

                var adminIdClaim = principal.FindFirst("AdminId")?.Value ?? principal.FindFirst("Id")?.Value ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                
                if (string.IsNullOrEmpty(adminIdClaim))
                    return null;

                if (int.TryParse(adminIdClaim, out var adminId))
                    return adminId;

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra token có hợp lệ không
        /// </summary>
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var principal = ValidateToken(token);
            return principal != null;
        }
    }
}
