namespace GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs
{
    public class OutfitAIResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public Guid? GeneratedOutfitId { get; set; }
        public List<OutfitItemDto> OutfitItems { get; set; } = new List<OutfitItemDto>();
        public string? RecommendationReason { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }

    public class OutfitItemDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string? ItemImageUrl { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? ColorName { get; set; }
        public string? Brand { get; set; }
        public string? Size { get; set; }
        public string ItemType { get; set; } = null!;
        public int PositionOrder { get; set; }
    }
} 