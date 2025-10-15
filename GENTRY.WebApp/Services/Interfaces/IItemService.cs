using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.ItemDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<Item>> GetAllItems();
        Task<List<ItemDto>> GetItemsByUserIdAsync(Guid userId);
        Task<List<ItemDto>> GetMyItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(Guid itemId);
        Task<ItemDto> AddItemAsync(AddItemRequest request);
        Task<ItemDto> UpdateItemAsync(Guid itemId, UpdateItemRequest request);
        Task<bool> DeleteItemAsync(Guid itemId);
    }
}
