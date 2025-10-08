using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IAffiliateService
    {
        Task<List<AffiliateItemDto>> FindAffiliateProductsAsync(OutfitSuggestionDto outfit, decimal? maxBudget = null, string? preferredSize = null);
        Task<List<AffiliateItemDto>> SearchShopeeProductsAsync(string keywords, string category, decimal? maxPrice = null);
        Task<List<AffiliateItemDto>> SearchLazadaProductsAsync(string keywords, string category, decimal? maxPrice = null);
    }
}


