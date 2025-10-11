using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IGeminiAIService
    {
        Task<AiStylingResponseDto> GenerateOutfitSuggestionsAsync(AiStylingRequestDto request);
        Task<string?> GenerateOutfitImageAsync(OutfitSuggestionDto outfit);
        Task<string> AnalyzeUserPreferencesAsync(Guid userId);
        
        /// <summary>
        /// Tạo outfit recommendation từ các items có sẵn trong tủ đồ của người dùng sử dụng Gemini AI
        /// </summary>
        /// <param name="request">Yêu cầu chatbot từ người dùng</param>
        /// <returns>Response chứa outfit được đề xuất từ items có sẵn</returns>
        Task<OutfitAIResponseDto> GenerateOutfitFromWardrobeAsync(OutfitAIRequestDto request);
    }
}


