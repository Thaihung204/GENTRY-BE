using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IGeminiAIService
    {
        
        /// <summary>
        /// Tạo outfit recommendation từ các items có sẵn trong tủ đồ của người dùng sử dụng Gemini AI
        /// </summary>
        /// <param name="request">Yêu cầu chatbot từ người dùng</param>
        /// <param name="userId">ID của người dùng đã đăng nhập</param>
        /// <returns>Response chứa outfit được đề xuất từ items có sẵn</returns>
        Task<OutfitAIResponseDto> GenerateOutfitFromWardrobeAsync(OutfitAIRequestDto request, Guid userId);
    }
}


