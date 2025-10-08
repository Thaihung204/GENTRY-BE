using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GENTRY.WebApp.Services.Services
{
    public class AffiliateService : BaseService, IAffiliateService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AffiliateService> _logger;

        public AffiliateService(
            IRepository repository,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<AffiliateService> logger) : base(repository, httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<AffiliateItemDto>> FindAffiliateProductsAsync(OutfitSuggestionDto outfit, decimal? maxBudget = null, string? preferredSize = null)
        {
            var results = new List<AffiliateItemDto>();
            var budgetPerItem = maxBudget.HasValue && outfit.Items.Any() ? maxBudget.Value / Math.Max(outfit.Items.Count, 1) : (decimal?)null;

            // Prototype: Use search keywords from each item if available; otherwise use item name
            foreach (var desired in outfit.Items)
            {
                var shopee = await SearchShopeeProductsAsync(desired.ItemName, desired.CategoryName, budgetPerItem);
                var lazada = await SearchLazadaProductsAsync(desired.ItemName, desired.CategoryName, budgetPerItem);
                var best = shopee.Concat(lazada).OrderByDescending(i => (i.Rating ?? 0) + Math.Min(i.SoldCount ?? 0, 2000) / 2000.0).FirstOrDefault();
                if (best != null) results.Add(best);
            }

            return results;
        }

        public async Task<List<AffiliateItemDto>> SearchShopeeProductsAsync(string keywords, string category, decimal? maxPrice = null)
        {
            try
            {
                // Mock realistic data for demo
                await Task.Delay(50);
                var list = new List<AffiliateItemDto>
                {
                    new AffiliateItemDto
                    {
                        ItemId = "sp_001",
                        ItemName = $"{keywords} - Chất liệu thoáng mát",
                        ItemImageUrl = "https://cf.shopee.vn/file/4d1f2f-placeholder.jpg",
                        CategoryName = category,
                        Brand = "LocalBrand A",
                        Color = "Đen",
                        Size = "S,M,L",
                        Price = maxPrice.HasValue ? Math.Min(299000, maxPrice.Value) : 299000,
                        Currency = "VND",
                        Platform = "Shopee",
                        AffiliateUrl = $"https://shopee.vn/search?keyword={Uri.EscapeDataString(keywords)}",
                        CommissionRate = decimal.Parse(_configuration["Shopee:DefaultCommissionRate"] ?? "3.0"),
                        ShopName = "Shop Thời Trang ABC",
                        Rating = 4.6f,
                        SoldCount = 1200,
                        IsOnSale = true,
                        OriginalPrice = 349000,
                        DiscountPercent = 15
                    },
                    new AffiliateItemDto
                    {
                        ItemId = "sp_002",
                        ItemName = $"{keywords} - Form rộng trendy",
                        ItemImageUrl = "https://cf.shopee.vn/file/7a2b3c-placeholder.jpg",
                        CategoryName = category,
                        Brand = "LocalBrand B",
                        Color = "Trắng",
                        Size = "M,L,XL",
                        Price = maxPrice.HasValue ? Math.Min(259000, maxPrice.Value) : 259000,
                        Currency = "VND",
                        Platform = "Shopee",
                        AffiliateUrl = $"https://shopee.vn/search?keyword={Uri.EscapeDataString(keywords)}",
                        CommissionRate = decimal.Parse(_configuration["Shopee:DefaultCommissionRate"] ?? "3.0"),
                        ShopName = "Fashion House XYZ",
                        Rating = 4.3f,
                        SoldCount = 860,
                        IsOnSale = false
                    }
                };

                return list.Where(p => !maxPrice.HasValue || p.Price <= maxPrice.Value).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Shopee mock data");
                return new List<AffiliateItemDto>();
            }
        }

        public async Task<List<AffiliateItemDto>> SearchLazadaProductsAsync(string keywords, string category, decimal? maxPrice = null)
        {
            try
            {
                // Mock realistic data for demo
                await Task.Delay(50);
                var list = new List<AffiliateItemDto>
                {
                    new AffiliateItemDto
                    {
                        ItemId = "lz_001",
                        ItemName = $"{keywords} - Vải cotton premium",
                        ItemImageUrl = "https://lzd-img-global.slatic.net/g/placeholder.jpg",
                        CategoryName = category,
                        Brand = "Lazada Choice",
                        Color = "Xanh navy",
                        Size = "M,L",
                        Price = maxPrice.HasValue ? Math.Min(279000, maxPrice.Value) : 279000,
                        Currency = "VND",
                        Platform = "Lazada",
                        AffiliateUrl = $"https://www.lazada.vn/catalog/?q={Uri.EscapeDataString(keywords)}",
                        CommissionRate = decimal.Parse(_configuration["Lazada:DefaultCommissionRate"] ?? "2.5"),
                        ShopName = "LazMall Official",
                        Rating = 4.7f,
                        SoldCount = 2400,
                        IsOnSale = true,
                        OriginalPrice = 329000,
                        DiscountPercent = 15
                    }
                };

                return list.Where(p => !maxPrice.HasValue || p.Price <= maxPrice.Value).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Lazada mock data");
                return new List<AffiliateItemDto>();
            }
        }
    }
}


