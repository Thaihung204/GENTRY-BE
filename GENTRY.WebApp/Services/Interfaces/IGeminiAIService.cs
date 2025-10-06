using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IGeminiAIService
    {
        Task<AiStylingResponseDto> GenerateOutfitSuggestionsAsync(AiStylingRequestDto request);
        Task<string?> GenerateOutfitImageAsync(OutfitSuggestionDto outfit);
        Task<string> AnalyzeUserPreferencesAsync(Guid userId);
    }
}


