using AutoMapper;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.CategoryDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IMapper _mapper;

        public CategoryService(IRepository repo, IMapper mapper) : base(repo)
        {
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            try
            {
                var categories = await Repo.GetAsync<Category>();
                return categories.ToList();
            }
            catch
            {
                return new List<Category>();
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                var category = await Repo.GetByIdAsync<Category>(id);
                return category;
            }
            catch
            {
                return null;
            }
        }

        public async Task<CategoryDto> CreateAsync(AddCategoryRequestDto request)
        {
            try
            {
                var category = _mapper.Map<Category>(request);
                category.CreatedDate = DateTime.UtcNow;
                
                await Repo.CreateAsync(category);
                await Repo.SaveAsync();
                
                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating category: {ex.Message}", ex);
            }
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryRequestDto request)
        {
            try
            {
                var existingCategory = await Repo.GetByIdAsync<Category>(id);
                if (existingCategory == null)
                {
                    return null;
                }

                // Manually update properties to preserve ID
                existingCategory.Name = request.Name;
                existingCategory.Description = request.Description;
                existingCategory.ParentId = request.ParentId;
                existingCategory.ImageFileId = request.ImageFileId;
                existingCategory.IsActive = request.IsActive;
                existingCategory.SortOrder = request.SortOrder;
                existingCategory.ModifiedDate = DateTime.UtcNow;
                
                Repo.Update(existingCategory);
                await Repo.SaveAsync();
                
                return _mapper.Map<CategoryDto>(existingCategory);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating category: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var category = await Repo.GetByIdAsync<Category>(id);
                if (category == null)
                {
                    return false;
                }

                Repo.Delete(category);
                await Repo.SaveAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
