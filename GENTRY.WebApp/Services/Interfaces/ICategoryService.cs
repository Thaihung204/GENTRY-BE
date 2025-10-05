using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.CategoryDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface ICategoryService
    { 
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(AddCategoryRequestDto request);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
