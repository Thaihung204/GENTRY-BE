using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IChatHistoryService
    {
        /// <summary>
        /// Lấy lịch sử chat của người dùng hiện tại
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <param name="limit">Giới hạn số lượng tin nhắn (mặc định 50)</param>
        /// <returns>Danh sách lịch sử chat</returns>
        Task<List<ChatHistoryDto>> GetChatHistoryAsync(Guid userId, int limit = 50);

        /// <summary>
        /// Xóa lịch sử chat cụ thể
        /// </summary>
        /// <param name="chatId">ID của chat cần xóa</param>
        /// <param name="userId">ID của người dùng (để kiểm tra quyền)</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteChatAsync(Guid chatId, Guid userId);

        /// <summary>
        /// Xóa toàn bộ lịch sử chat của người dùng
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> ClearAllChatHistoryAsync(Guid userId);
    }

    public class ChatHistoryDto
    {
        public Guid Id { get; set; }
        public string UserMessage { get; set; } = null!;
        public string AiResponse { get; set; } = null!;
        public ParsedAiResponseDto? ParsedAi { get; set; }
        public string? Occasion { get; set; }
        public string? WeatherCondition { get; set; }
        public string? Season { get; set; }
        public string? AdditionalPreferences { get; set; }
        public Guid? GeneratedOutfitId { get; set; }
        public string ChatType { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }

    public class ParsedAiResponseDto
    {
        public bool Success { get; set; }
        public List<ParsedSelectedItemDto> SelectedItems { get; set; } = new();
        public string? OutfitDescription { get; set; }
        public string? StyleAnalysis { get; set; }
        public string? RecommendationReason { get; set; }
        public string? StylingTips { get; set; }
    }

    public class ParsedSelectedItemDto
    {
        public Guid? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Reason { get; set; }
    }
}
