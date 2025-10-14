using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GENTRY.WebApp.Services.Services
{
    public class ChatHistoryService : BaseService, IChatHistoryService
    {
        private readonly ILogger<ChatHistoryService> _logger;

        public ChatHistoryService(
            IRepository repository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ChatHistoryService> logger) : base(repository, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<List<ChatHistoryDto>> GetChatHistoryAsync(Guid userId, int limit = 50)
        {
            try
            {
                var limitValue = Math.Min(limit, 100); // Giới hạn tối đa 100 để tránh quá tải
                
                var chatHistories = await Repo.GetAsync<ChatHistory>(
                    filter: ch => ch.UserId == userId && ch.IsActive == true,
                    orderBy: q => q.OrderByDescending(ch => ch.CreatedDate),
                    take: limitValue
                );

                var result = new List<ChatHistoryDto>();

                foreach (var ch in chatHistories)
                {
                    var dto = new ChatHistoryDto
                    {
                        Id = ch.Id,
                        UserMessage = ch.UserMessage,
                        AiResponse = ch.AiResponse,
                        Occasion = ch.Occasion,
                        WeatherCondition = ch.WeatherCondition,
                        Season = ch.Season,
                        AdditionalPreferences = ch.AdditionalPreferences,
                        GeneratedOutfitId = ch.GeneratedOutfitId,
                        ChatType = ch.ChatType,
                        CreatedDate = ch.CreatedDate
                    };

                    // Parse AI response if possible
                    dto.ParsedAi = TryParseAiResponse(ch.AiResponse);

                    result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat history for user {UserId}", userId);
                return new List<ChatHistoryDto>();
            }
        }

        private ParsedAiResponseDto? TryParseAiResponse(string aiResponse)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(aiResponse)) return null;

                // Strip markdown code fences if present
                var cleaned = aiResponse
                    .Replace("```json\n", string.Empty)
                    .Replace("```", string.Empty)
                    .Trim();

                dynamic? parsed = Newtonsoft.Json.JsonConvert.DeserializeObject(cleaned);
                if (parsed == null) return null;

                var result = new ParsedAiResponseDto
                {
                    Success = (bool?)parsed.success ?? true,
                    OutfitDescription = (string?)parsed.outfitDescription,
                    StyleAnalysis = (string?)parsed.styleAnalysis,
                    RecommendationReason = (string?)parsed.recommendationReason,
                    StylingTips = (string?)parsed.stylingTips
                };

                var items = new List<ParsedSelectedItemDto>();
                if (parsed.selectedItems != null)
                {
                    foreach (var it in parsed.selectedItems)
                    {
                        Guid? itemId = null;
                        try
                        {
                            var idStr = (string?)it.itemId;
                            if (!string.IsNullOrEmpty(idStr) && Guid.TryParse(idStr, out var gid))
                            {
                                itemId = gid;
                            }
                        }
                        catch { /* ignore */ }

                        items.Add(new ParsedSelectedItemDto
                        {
                            ItemId = itemId,
                            ItemName = (string?)it.itemName,
                            Reason = (string?)it.reason
                        });
                    }
                }
                result.SelectedItems = items;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse AI response JSON");
                return null;
            }
        }

        public async Task<bool> DeleteChatAsync(Guid chatId, Guid userId)
        {
            try
            {
                var chatHistory = await Repo.GetOneAsync<ChatHistory>(
                    ch => ch.Id == chatId && ch.UserId == userId && ch.IsActive == true);

                if (chatHistory == null)
                {
                    _logger.LogWarning("Chat {ChatId} not found or not owned by user {UserId}", chatId, userId);
                    return false;
                }

                // Soft delete - chỉ đánh dấu IsActive = false
                chatHistory.IsActive = false;
                chatHistory.ModifiedDate = DateTime.UtcNow;

                Repo.Update(chatHistory);
                await Repo.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat {ChatId} for user {UserId}", chatId, userId);
                return false;
            }
        }

        public async Task<bool> ClearAllChatHistoryAsync(Guid userId)
        {
            try
            {
                var chatHistories = await Repo.GetAsync<ChatHistory>(
                    filter: ch => ch.UserId == userId && ch.IsActive == true
                );

                var chatHistoryList = chatHistories.ToList();
                
                foreach (var chat in chatHistoryList)
                {
                    chat.IsActive = false;
                    chat.ModifiedDate = DateTime.UtcNow;
                    Repo.Update(chat);
                }

                if (chatHistoryList.Any())
                {
                    await Repo.SaveAsync();
                }

                _logger.LogInformation("Cleared {Count} chat histories for user {UserId}", chatHistoryList.Count, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all chat history for user {UserId}", userId);
                return false;
            }
        }
    }
}
