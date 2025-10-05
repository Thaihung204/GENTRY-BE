using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.FileDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileUploadResult> UploadImageAsync(IFormFile imageFile, string folder = "items");
        Task<bool> DeleteImageAsync(string publicId);
        Task<Models.File> SaveFileInfoAsync(string name, string url, Guid uploadedBy);
    }
} 