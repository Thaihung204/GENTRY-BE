using AutoMapper;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.ItemDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class ItemService : BaseService, IItemService
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ItemService(IRepository repo, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFileService fileService) 
            : base(repo, httpContextAccessor)
        {
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<List<Item>> GetAllItems()
        {
            try
            {
                var items = await Repo.GetAsync<Item>(
                    orderBy: q => q.OrderByDescending(i => i.CreatedDate),
                    includeProperties: "Category,File,Color,User"
                );

                return items.ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }

        public async Task<List<ItemDto>> GetItemsByUserIdAsync(Guid userId)
        {
            try
            {
                var items = await Repo.GetAsync<Item>(
                    filter: i => i.UserId == userId ,
                    orderBy: q => q.OrderByDescending(i => i.CreatedDate),
                    includeProperties: "Category,File,Color"
                );

                var itemDtos = _mapper.Map<List<ItemDto>>(items);
                return itemDtos;
            }
            catch
            {
                return new List<ItemDto>();
            }
        }

        public async Task<List<ItemDto>> GetMyItemsAsync()
        {
            try
            {
                var currentUserId = UserId;
                return await GetItemsByUserIdAsync(currentUserId);
            }
            catch
            {
                return new List<ItemDto>();
            }
        }

        public async Task<ItemDto?> GetItemByIdAsync(Guid itemId)
        {
            try
            {
                var currentUserId = UserId;
                
                var items = await Repo.GetAsync<Item>(
                    filter: i => i.Id == itemId && i.UserId == currentUserId,
                    includeProperties: "Category,File,Color"
                );

                var item = items.FirstOrDefault();
                if (item == null)
                {
                    return null;
                }

                var itemDto = _mapper.Map<ItemDto>(item);
                return itemDto;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ItemDto> AddItemAsync(AddItemRequest request)
        {
            try
            {
                var currentUserId = UserId;
                int? fileId = null;

                // Xử lý upload ảnh nếu có
                if (request.ImageFile != null)
                {
                    // Upload ảnh lên Cloudinary
                    var uploadResult = await _fileService.UploadImageAsync(request.ImageFile, "gentry-items");
                    
                    if (!uploadResult.Success)
                    {
                        throw new Exception($"Lỗi upload ảnh: {uploadResult.ErrorMessage}");
                    }

                    // Lưu thông tin file vào database
                    var savedFile = await _fileService.SaveFileInfoAsync(
                        request.ImageFile.FileName,
                        uploadResult.Url!,
                        currentUserId
                    );

                    fileId = savedFile.Id;
                }
                
                // Sử dụng AutoMapper để tạo item mới từ request
                var newItem = _mapper.Map<Item>(request);
                newItem.UserId = currentUserId;
                newItem.FileId = fileId;
                newItem.CreatedBy = currentUserId.ToString();
                newItem.CreatedDate = DateTime.UtcNow;

                // Lưu vào database
                await Repo.CreateAsync(newItem);

                // Lấy lại item với thông tin đầy đủ
                var createdItem = await Repo.GetAsync<Item>(
                    filter: i => i.Id == newItem.Id,
                    includeProperties: "Category,File,Color"
                );

                var item = createdItem.FirstOrDefault();
                if (item == null)
                {
                    throw new Exception("Không thể tạo item mới");
                }

                // Sử dụng AutoMapper để chuyển đổi sang DTO
                var itemDto = _mapper.Map<ItemDto>(item);
                return itemDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo item mới: {ex.Message}");
            }
        }

        public async Task<ItemDto> UpdateItemAsync(Guid itemId, UpdateItemRequest request)
        {
            try
            {
                var currentUserId = UserId;

                // Kiểm tra item có tồn tại và thuộc về user hiện tại không
                var existingItems = await Repo.GetAsync<Item>(
                    filter: i => i.Id == itemId && i.UserId == currentUserId
                );

                var existingItem = existingItems.FirstOrDefault();
                if (existingItem == null)
                {
                    throw new Exception("Không tìm thấy trang phục hoặc bạn không có quyền chỉnh sửa");
                }

                // Xử lý upload ảnh mới nếu có
                int? fileId = existingItem.FileId; // Giữ nguyên file cũ nếu không upload mới
                
                if (request.ImageFile != null)
                {
                    // Upload ảnh mới lên Cloudinary
                    var uploadResult = await _fileService.UploadImageAsync(request.ImageFile, "gentry-items");
                    
                    if (!uploadResult.Success)
                    {
                        throw new Exception($"Lỗi upload ảnh: {uploadResult.ErrorMessage}");
                    }

                    // Lưu thông tin file mới vào database
                    var savedFile = await _fileService.SaveFileInfoAsync(
                        request.ImageFile.FileName,
                        uploadResult.Url!,
                        currentUserId
                    );

                    fileId = savedFile.Id;
                }

                // Cập nhật thông tin item
                existingItem.Name = request.Name;
                existingItem.CategoryId = request.CategoryId;
                existingItem.Brand = request.Brand;
                existingItem.ColorId = request.ColorId;
                existingItem.Tags = request.Tags;
                existingItem.FileId = fileId;
                existingItem.ModifiedBy = currentUserId.ToString();
                existingItem.ModifiedDate = DateTime.UtcNow;

                // Cập nhật vào database
                Repo.Update(existingItem, currentUserId.ToString());
                await Repo.SaveAsync();

                // Lấy lại item với thông tin đầy đủ
                var updatedItems = await Repo.GetAsync<Item>(
                    filter: i => i.Id == itemId,
                    includeProperties: "Category,File,Color"
                );

                var updatedItem = updatedItems.FirstOrDefault();
                if (updatedItem == null)
                {
                    throw new Exception("Không thể cập nhật trang phục");
                }

                // Sử dụng AutoMapper để chuyển đổi sang DTO
                var itemDto = _mapper.Map<ItemDto>(updatedItem);
                return itemDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trang phục: {ex.Message}");
            }
        }

        public async Task<bool> DeleteItemAsync(Guid itemId)
        {
            try
            {
                var currentUserId = UserId;

                // Kiểm tra item có tồn tại và thuộc về user hiện tại không
                var existingItems = await Repo.GetAsync<Item>(
                    filter: i => i.Id == itemId && i.UserId == currentUserId
                );

                var existingItem = existingItems.FirstOrDefault();
                if (existingItem == null)
                {
                    return false; // Item không tồn tại hoặc không có quyền xóa
                }

                // Xóa item khỏi database
                Repo.Delete(existingItem);
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
