namespace GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs
{
    public class AiStylingResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<OutfitSuggestionDto> OutfitSuggestions { get; set; } = new List<OutfitSuggestionDto>();
        public string? AiAnalysis { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }

    public class OutfitSuggestionDto
    {
        public string OutfitId { get; set; } = null!;
        public string OutfitName { get; set; } = null!;
        public string? OutfitImageUrl { get; set; }
        public string StyleDescription { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public List<AffiliateItemDto> Items { get; set; } = new List<AffiliateItemDto>();
        public string? MatchingReason { get; set; }
        public int ConfidenceScore { get; set; }
    }

    public class AffiliateItemDto
    {
        public string ItemId { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string? ItemImageUrl { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "VND";
        public string Platform { get; set; } = null!;
        public string AffiliateUrl { get; set; } = null!;
        public decimal CommissionRate { get; set; }
        public string? ShopName { get; set; }
        public float? Rating { get; set; }
        public int? SoldCount { get; set; }
        public bool IsOnSale { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int? DiscountPercent { get; set; }
    }
}


