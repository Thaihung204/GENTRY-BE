using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.FileDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class CloudinaryService : BaseService, IFileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public CloudinaryService(
            IRepository repo, 
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration) 
            : base(repo, httpContextAccessor)
        {
            _configuration = configuration;
            
            var cloudName = _configuration["Cloudinary:CloudName"];
            var apiKey = _configuration["Cloudinary:ApiKey"];
            var apiSecret = _configuration["Cloudinary:ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new ArgumentException("Cloudinary configuration is missing. Please check appsettings.json");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<FileUploadResult> UploadImageAsync(IFormFile imageFile, string folder = "items")
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return new FileUploadResult 
                    { 
                        Success = false, 
                        ErrorMessage = "File không hợp lệ hoặc trống" 
                    };
                }

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    return new FileUploadResult 
                    { 
                        Success = false, 
                        ErrorMessage = "Chỉ hỗ trợ các định dạng: JPG, JPEG, PNG, GIF, WEBP" 
                    };
                }

                // Kiểm tra kích thước file (max 10MB)
                if (imageFile.Length > 10 * 1024 * 1024)
                {
                    return new FileUploadResult 
                    { 
                        Success = false, 
                        ErrorMessage = "Kích thước file không được vượt quá 10MB" 
                    };
                }

                using var stream = imageFile.OpenReadStream();
                
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = folder,
                    UniqueFilename = true,
                    Overwrite = false,
                    Transformation = new Transformation()
                        .Quality("auto:good")
                        .FetchFormat("auto")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return new FileUploadResult 
                    { 
                        Success = false, 
                        ErrorMessage = $"Lỗi upload: {uploadResult.Error.Message}" 
                    };
                }

                return new FileUploadResult
                {
                    Success = true,
                    Url = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    FileSize = uploadResult.Bytes,
                    Format = uploadResult.Format
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResult 
                { 
                    Success = false, 
                    ErrorMessage = $"Lỗi không xác định: {ex.Message}" 
                };
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                if (string.IsNullOrEmpty(publicId))
                    return false;

                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }

        public async Task<Models.File> SaveFileInfoAsync(string name, string url, Guid uploadedBy)
        {
            try
            {
                var file = new Models.File
                {
                    Name = name,
                    Url = url,
                    UploadedBy = uploadedBy,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = uploadedBy.ToString()
                };

                await Repo.CreateAsync(file);
                return file;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu thông tin file: {ex.Message}");
            }
        }
    }
} 