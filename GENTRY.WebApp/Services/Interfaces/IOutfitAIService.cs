using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IOutfitAIService
    {
        /// <summary>
        /// Tạo outfit recommendation dựa trên yêu cầu của người dùng
        /// </summary>
        /// <param name="request">Yêu cầu từ người dùng</param>
        /// <returns>Response chứa outfit được đề xuất và hình ảnh</returns>
        Task<OutfitAIResponseDto> GenerateOutfitRecommendationAsync(OutfitAIRequestDto request);

        /// <summary>
        /// Tạo hình ảnh outfit từ các items được chọn
        /// </summary>
        /// <param name="outfitItems">Các items trong outfit</param>
        /// <param name="userId">ID của user</param>
        /// <returns>URL của hình ảnh được tạo</returns>
        Task<string?> GenerateOutfitImageAsync(List<OutfitItemDto> outfitItems, Guid userId);

        /// <summary>
        /// Lưu outfit được tạo bởi AI vào database
        /// </summary>
        /// <param name="outfitData">Dữ liệu outfit</param>
        /// <param name="userId">ID của user</param>
        /// <returns>ID của outfit đã tạo</returns>
        Task<Guid> SaveGeneratedOutfitAsync(List<OutfitItemDto> outfitItems, Guid userId, string description, string? occasion = null, string? weather = null, string? season = null);
    }
} 