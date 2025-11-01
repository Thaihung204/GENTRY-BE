using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace GENTRY.WebApp.Services.Services
{
    public class GeminiAIService : BaseService, IGeminiAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiAIService> _logger;
        private readonly IItemService _itemService;
        private readonly string _geminiApiKey;
        private readonly string _geminiEndpoint;

        public GeminiAIService(
            IRepository repository,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiAIService> logger,
            IItemService itemService) : base(repository, httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _itemService = itemService;

            _geminiApiKey = _configuration["Gemini:ApiKey"]
                ?? throw new InvalidOperationException("Gemini API key not configured");
            _geminiEndpoint = _configuration["Gemini:Endpoint"]
                ?? "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";
        }

       

       

        private async Task<string> CallGeminiApiAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _geminiApiKey);

            var response = await _httpClient.PostAsync(_geminiEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API error: {Status} - {Body}", response.StatusCode, responseContent);
                throw new HttpRequestException($"Gemini API call failed: {response.StatusCode}");
            }

            var data = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return data?.candidates?[0]?.content?.parts?[0]?.text ?? string.Empty;
        }

        
        public async Task<OutfitAIResponseDto> GenerateOutfitFromWardrobeAsync(OutfitAIRequestDto request, Guid userId)
        {
            try
            {
                // 1️⃣ Lấy items của user từ tủ đồ
                var userItemsFromService = await _itemService.GetItemsByUserIdAsync(userId);
                if (userItemsFromService == null || !userItemsFromService.Any())
                {
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Bạn chưa có items nào trong tủ đồ. Vui lòng thêm items trước khi sử dụng tính năng này."
                    };
                }

                var userItems = userItemsFromService.Select(item => new OutfitItemDto
                {
                    ItemId = item.Id,
                    ItemName = item.Name ?? "Không rõ tên",
                    ItemImageUrl = item.FileUrl ?? "",
                    CategoryName = item.CategoryName ?? "",
                    ColorName = item.ColorName ?? "Không rõ màu",
                    Brand = item.Brand ?? "Không rõ thương hiệu",
                    Size = item.Size,
                    ItemType = item.CategoryName ?? ""
                }).ToList();

                // 2️⃣ Lấy thông tin user để cá nhân hóa
                var user = await Repo.GetOneAsync<User>(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("Không tìm thấy user {UserId}", userId);
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Không tìm thấy thông tin người dùng."
                    };
                }

                // 3️⃣ Tạo prompt cho Gemini AI
                var prompt = BuildWardrobeOutfitPrompt(request, userItems, user);

                // 4️⃣ Gọi Gemini API (có try/catch bên trong)
                string aiResponse = string.Empty;
                try
                {
                    aiResponse = await CallGeminiApiAsync(prompt);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi GENTRY API");
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Không thể kết nối tới dịch vụ GENTRY AI. Vui lòng thử lại sau."
                    };
                }

                // 5️⃣ Parse response AI
                var selectedItems = new List<OutfitItemDto>();
                try
                {
                    selectedItems = ParseGeminiWardrobeResponse(aiResponse, userItems) ?? new List<OutfitItemDto>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi phân tích kết quả GENTRY AI");
                }

                if (!selectedItems.Any())
                {
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Không thể tạo outfit phù hợp từ các items hiện có. Vui lòng thử lại."
                    };
                }

                // 6️⃣ Sinh ảnh outfit (an toàn)
                string imageUrl;
                try
                {
                    imageUrl = await GenerateWardrobeOutfitImageAsync(selectedItems);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Lỗi khi tạo ảnh outfit, dùng ảnh mặc định");
                    imageUrl = "https://placehold.co/400x400?text=Outfit+Preview";
                }

                // 7️⃣ Lưu outfit vào database
                var outfitId = Guid.Empty;
                try
                {
                    outfitId = await SaveGeneratedOutfitAsync(
                        selectedItems,
                        userId,
                        $"Gemini AI Generated: {request.UserMessage}"
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Lỗi khi lưu outfit vào DB");
                }

                // 8️⃣ Lưu chat message vào database
                try
                {
                    await SaveChatMessageAsync(userId, request.UserMessage, aiResponse);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Lỗi khi lưu chat message vào DB");
                }

                return new OutfitAIResponseDto
                {
                    Success = true,
                    Message = "Đã tạo outfit từ tủ đồ của bạn bằng GENTRY AI!",
                    ImageUrl = imageUrl,
                    GeneratedOutfitId = outfitId,
                    OutfitItems = selectedItems,
                    RecommendationReason = ExtractGeminiRecommendationReason(aiResponse)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit from wardrobe for user {UserId}", userId);
                return new OutfitAIResponseDto
                {
                    Success = false,
                    Message = "Có lỗi hệ thống xảy ra khi tạo outfit. Vui lòng thử lại sau."
                };
            }
        }

        private string BuildWardrobeOutfitPrompt(OutfitAIRequestDto request, List<OutfitItemDto> userItems, User user)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("Bạn là một stylist chuyên nghiệp sử dụng GENTRY AI. Nhiệm vụ của bạn là tạo outfit hoàn hảo từ các items có sẵn trong tủ đồ của khách hàng.");
            prompt.AppendLine();

            // Thông tin khách hàng
            prompt.AppendLine("THÔNG TIN KHÁCH HÀNG:");
            prompt.AppendLine($"- Giới tính: {user?.Gender ?? "Không xác định"}");
            if (user?.BirthDate.HasValue == true)
                prompt.AppendLine($"- Tuổi: {DateTime.Now.Year - user.BirthDate.Value.Year}");
            prompt.AppendLine($"- Sở thích phong cách: {user?.StylePreferences ?? "Chưa xác định"}");
            prompt.AppendLine($"- Vóc dáng: {user?.BodyType ?? "Không xác định"}");
            prompt.AppendLine($"- Tông da: {user?.SkinTone ?? "Không xác định"}");
            if (user?.Height.HasValue == true)
                prompt.AppendLine($"- Chiều cao: {user.Height}cm");
            if (user?.Weight.HasValue == true)
                prompt.AppendLine($"- Cân nặng: {user.Weight}kg");
            prompt.AppendLine();

            // Yêu cầu cụ thể
            prompt.AppendLine("YÊU CẦU TỪ KHÁCH HÀNG:");
            prompt.AppendLine($"- Mô tả: {request.UserMessage}");
            prompt.AppendLine();

            // Danh sách items có sẵn
            prompt.AppendLine("CÁC ITEMS CÓ SẴN TRONG TỦ ĐỒ:");
            foreach (var item in userItems)
            {
                prompt.AppendLine($"- ID: {item.ItemId}");
                prompt.AppendLine($"  Tên: {item.ItemName}");
                prompt.AppendLine($"  Loại: {item.CategoryName}");
                prompt.AppendLine($"  Màu sắc: {item.ColorName ?? "Không xác định"}");
                prompt.AppendLine($"  Thương hiệu: {item.Brand ?? "Không có"}");
                prompt.AppendLine($"  Size: {item.Size ?? "Không xác định"}");
                prompt.AppendLine();
            }

            prompt.AppendLine("NHIỆM VỤ CỦA BẠN:");
            prompt.AppendLine("1. Phân tích kỹ lưỡng yêu cầu và đặc điểm của khách hàng");
            prompt.AppendLine("2. Xem xét tất cả items có sẵn trong tủ đồ");
            prompt.AppendLine("3. Chọn những items phù hợp nhất để tạo outfit hoàn chỉnh, hài hòa và thời trang");
            prompt.AppendLine("4. Đảm bảo outfit phù hợp với dịp, thời tiết và phong cách mong muốn");
            prompt.AppendLine("5. Giải thích chi tiết lý do lựa chọn và cách phối đồ");
            prompt.AppendLine();

            prompt.AppendLine("YÊU CẦU ĐỊNH DẠNG PHẢN HỒI (JSON thuần túy, không có markdown):");
            prompt.AppendLine("{");
            prompt.AppendLine("  \"success\": true,");
            prompt.AppendLine("  \"selectedItems\": [");
            prompt.AppendLine("    {");
            prompt.AppendLine("      \"itemId\": \"guid-của-item\",");
            prompt.AppendLine("      \"itemName\": \"tên-item\",");
            prompt.AppendLine("      \"reason\": \"lý do chọn item này\"");
            prompt.AppendLine("    }");
            prompt.AppendLine("  ],");
            prompt.AppendLine("  \"outfitDescription\": \"mô tả tổng thể về outfit\",");
            prompt.AppendLine("  \"styleAnalysis\": \"phân tích phong cách của outfit\",");
            prompt.AppendLine("  \"recommendationReason\": \"lý do tổng thể tại sao outfit này phù hợp\",");
            prompt.AppendLine("  \"stylingTips\": \"gợi ý cách mặc và phối thêm phụ kiện\"");
            prompt.AppendLine("}");

            return prompt.ToString();
        }

        private List<OutfitItemDto> ParseGeminiWardrobeResponse(string aiResponse, List<OutfitItemDto> availableItems)
        {
            try
            {
                // Parse JSON response từ Gemini AI
                var responseData = JsonConvert.DeserializeObject<dynamic>(aiResponse);
                var selectedItemsJson = responseData?.selectedItems;

                var selectedItems = new List<OutfitItemDto>();

                if (selectedItemsJson != null)
                {
                    foreach (var selectedItem in selectedItemsJson)
                    {
                        if (Guid.TryParse(selectedItem.itemId.ToString(), out Guid itemId))
                        {
                            var item = availableItems.FirstOrDefault(i => i.ItemId == itemId);
                            if (item != null)
                            {
                                selectedItems.Add(item);
                            }
                        }
                    }
                }

                return selectedItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing Gemini wardrobe response: {Response}", aiResponse);

                // Fallback: chọn một số items ngẫu nhiên nếu không parse được
                var random = new Random();
                return availableItems.OrderBy(x => random.Next()).Take(Math.Min(3, availableItems.Count)).ToList();
            }
        }

        private async Task<string?> GenerateWardrobeOutfitImageAsync(List<OutfitItemDto> selectedItems)
        {
            try
            {
                // Tạo description cho outfit image
                var itemNames = string.Join(", ", selectedItems.Select(i => i.ItemName));
                var imagePrompt = $"Professional fashion photography of an outfit containing: {itemNames}. Clean background, good lighting, fashion styling.";
                
                // Placeholder - có thể tích hợp với AI image generation service sau
                await Task.Delay(100);
                
                // Return URL của item đầu tiên có hình ảnh làm placeholder
                var firstItemWithImage = selectedItems.FirstOrDefault(i => !string.IsNullOrEmpty(i.ItemImageUrl));
                return firstItemWithImage?.ItemImageUrl ?? "https://via.placeholder.com/400x500/cccccc/666666?text=Outfit";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating wardrobe outfit image");
                return "https://via.placeholder.com/400x500/cccccc/666666?text=Outfit";
            }
        }

        private async Task<Guid> SaveGeneratedOutfitAsync(List<OutfitItemDto> outfitItems, Guid userId, string description)
        {
            var outfit = new Outfit
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = $"GENTRY AI Outfit {DateTime.Now:dd/MM/yyyy HH:mm}",
                Description = description,
                IsAiGenerated = true
            };

            await Repo.CreateAsync(outfit);
            await Repo.SaveAsync();

            // Thêm các items vào outfit
            foreach (var item in outfitItems.Select((item, index) => new { item, index }))
            {
                var outfitItem = new OutfitItem
                {
                    OutfitId = outfit.Id,
                    ItemId = item.item.ItemId,
                    ItemType = item.item.ItemType,
                    PositionOrder = item.index + 1
                };

                await Repo.CreateAsync(outfitItem);
            }

            await Repo.SaveAsync();
            return outfit.Id;
        }

        private string ExtractGeminiRecommendationReason(string aiResponse)
        {
            try
            {
                var responseData = JsonConvert.DeserializeObject<dynamic>(aiResponse);
                return responseData?.recommendationReason?.ToString() ?? 
                       responseData?.outfitDescription?.ToString() ?? 
                       "GENTRY AI đã tạo outfit dựa trên phân tích các items trong tủ đồ và sở thích của bạn.";
            }
            catch
            {
                return "GENTRY AI đã tạo outfit dựa trên phân tích các items trong tủ đồ và sở thích của bạn.";
            }
        }

        private async Task SaveChatMessageAsync(Guid userId, string userMessage, string aiResponse)
        {
            try
            {
                var chatMessage = new AIChatMessage
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    UserMessage = userMessage,
                    AIResponse = aiResponse,
                    CreatedAt = DateTime.UtcNow,
                    IsFromUser = true
                };

                await Repo.CreateAsync(chatMessage);
                await Repo.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving chat message for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<AIChatMessage>> GetChatHistoryAsync(Guid userId)
        {
            try
            {
                var chatHistory = await Repo.GetAsync<AIChatMessage>(
                    filter: cm => cm.UserId == userId,
                    orderBy: q => q.OrderByDescending(cm => cm.CreatedAt)
                );

                return chatHistory.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chat history for user {UserId}", userId);
                return new List<AIChatMessage>();
            }
        }
    }
}


