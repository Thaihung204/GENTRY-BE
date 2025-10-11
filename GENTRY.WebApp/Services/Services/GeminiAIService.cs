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
        private readonly IAffiliateService _affiliateService;
        private readonly IItemService _itemService;
        private readonly string _geminiApiKey;
        private readonly string _geminiEndpoint;

        public GeminiAIService(
            IRepository repository,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiAIService> logger,
            IAffiliateService affiliateService,
            IItemService itemService) : base(repository, httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _affiliateService = affiliateService;
            _itemService = itemService;

            _geminiApiKey = _configuration["Gemini:ApiKey"]
                ?? throw new InvalidOperationException("Gemini API key not configured");
            _geminiEndpoint = _configuration["Gemini:Endpoint"]
                ?? "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";
        }

        public async Task<AiStylingResponseDto> GenerateOutfitSuggestionsAsync(AiStylingRequestDto request)
        {
            try
            {
                var userPreferences = await GetUserPreferencesAsync(request.UserId);
                var contextData = await GetContextDataAsync(request);
                var prompt = BuildStylingPrompt(request, userPreferences, contextData);
                var aiText = await CallGeminiApiAsync(prompt);
                var outfits = ParseAIResponse(aiText);

                foreach (var suggestion in outfits)
                {
                    var affiliateItems = await _affiliateService.FindAffiliateProductsAsync(
                        suggestion, request.MaxBudget, request.PreferredSize);
                    suggestion.Items = affiliateItems;
                    suggestion.TotalPrice = affiliateItems.Sum(i => i.Price);
                    suggestion.OutfitImageUrl = await GenerateOutfitImageAsync(suggestion);
                }

                return new AiStylingResponseDto
                {
                    Success = true,
                    Message = "Đã tạo gợi ý outfit thành công!",
                    OutfitSuggestions = outfits,
                    AiAnalysis = ExtractAnalysisFromResponse(aiText)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit suggestions for user {UserId}", request.UserId);
                return new AiStylingResponseDto
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tạo gợi ý outfit. Vui lòng thử lại sau."
                };
            }
        }

        public async Task<string?> GenerateOutfitImageAsync(OutfitSuggestionDto outfit)
        {
            try
            {
                var imagePrompt = $"Fashion outfit: {outfit.StyleDescription}. Items: {string.Join(", ", outfit.Items.Select(i => i.ItemName))}. Professional fashion photography style, clean background.";
                await Task.Delay(10);
                return "https://images.example.com/outfit-placeholder.jpg";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit image");
                return null;
            }
        }

        public async Task<string> AnalyzeUserPreferencesAsync(Guid userId)
        {
            var outfits = await Repo.GetAsync<Outfit>(o => o.UserId == userId);
            var items = await Repo.GetAsync<Item>(i => i.UserId == userId);
            var prompt = BuildUserAnalysisPrompt(outfits.ToList(), items.ToList());
            return await CallGeminiApiAsync(prompt);
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

        private string BuildStylingPrompt(AiStylingRequestDto request, Dictionary<string, object> userPreferences, Dictionary<string, object> contextData)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia tư vấn thời trang AI. Tạo gợi ý outfit phù hợp.");
            sb.AppendLine("YÊU CẦU:");
            if (contextData.ContainsKey("categories")) sb.AppendLine($"- Danh mục: {contextData["categories"]}");
            if (contextData.ContainsKey("styles")) sb.AppendLine($"- Phong cách: {contextData["styles"]}");
            if (contextData.ContainsKey("occasion")) sb.AppendLine($"- Dịp: {contextData["occasion"]}");
            if (contextData.ContainsKey("weather")) sb.AppendLine($"- Thời tiết: {contextData["weather"]}");
            if (contextData.ContainsKey("colors")) sb.AppendLine($"- Màu sắc: {contextData["colors"]}");
            if (request.MaxBudget.HasValue) sb.AppendLine($"- Ngân sách tối đa: {request.MaxBudget:N0} VND");
            if (!string.IsNullOrEmpty(request.Gender)) sb.AppendLine($"- Giới tính: {request.Gender}");
            if (!string.IsNullOrEmpty(request.PreferredSize)) sb.AppendLine($"- Size: {request.PreferredSize}");
            if (!string.IsNullOrEmpty(request.AdditionalRequirements)) sb.AppendLine($"- Yêu cầu thêm: {request.AdditionalRequirements}");

            sb.AppendLine();
            sb.AppendLine($"Trả về JSON với {request.NumberOfSuggestions} outfit trong schema:");
            sb.AppendLine(@"{""outfits"":[{""outfitId"":"""",""outfitName"":"""",""styleDescription"":"""",""matchingReason"":"""",""confidenceScore"":0,""items"":[{""itemName"":"""",""categoryName"":"""",""color"":"""",""estimatedPrice"":0,""searchKeywords"":""""}]}], ""analysis"":""""}");
            
            return sb.ToString();
        }

        private async Task<Dictionary<string, object>> GetContextDataAsync(AiStylingRequestDto request)
        {
            var ctx = new Dictionary<string, object>();
            if (request.CategoryIds.Any())
            {
                var cats = await Repo.GetAsync<Category>(c => request.CategoryIds.Contains(c.Id));
                ctx["categories"] = string.Join(", ", cats.Select(c => c.Name));
            }
            if (request.StyleIds.Any())
            {
                var styles = await Repo.GetAsync<Style>(s => request.StyleIds.Contains(s.Id));
                ctx["styles"] = string.Join(", ", styles.Select(s => s.Name));
            }
            if (request.OccasionId.HasValue)
            {
                var occasion = await Repo.GetByIdAsync<Occasion>(request.OccasionId.Value);
                if (occasion != null) ctx["occasion"] = occasion.Name;
            }
            if (request.WeatherId.HasValue)
            {
                var weather = await Repo.GetByIdAsync<Weather>(request.WeatherId.Value);
                if (weather != null) ctx["weather"] = weather.Name;
            }
            if (request.ColorIds.Any())
            {
                var colors = await Repo.GetAsync<Color>(c => request.ColorIds.Contains(c.Id));
                ctx["colors"] = string.Join(", ", colors.Select(c => c.Name));
            }
            return ctx;
        }

        private async Task<Dictionary<string, object>> GetUserPreferencesAsync(Guid userId)
        {
            var user = await Repo.GetOneAsync<User>(u => u.Id == userId);
            var preferences = new Dictionary<string, object>();
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.BodyType)) preferences["bodyType"] = user.BodyType;
                if (!string.IsNullOrEmpty(user.StylePreferences)) preferences["stylePreferences"] = user.StylePreferences;
                if (!string.IsNullOrEmpty(user.SizePreferences)) preferences["sizePreferences"] = user.SizePreferences;
            }
            return preferences;
        }

        private List<OutfitSuggestionDto> ParseAIResponse(string aiText)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(aiText);
                var outfits = data?.outfits;
                var list = new List<OutfitSuggestionDto>();
                if (outfits != null)
                {
                    foreach (var o in outfits)
                    {
                        list.Add(new OutfitSuggestionDto
                        {
                            OutfitId = o?.outfitId ?? Guid.NewGuid().ToString(),
                            OutfitName = o?.outfitName ?? "Outfit Suggestion",
                            StyleDescription = o?.styleDescription ?? string.Empty,
                            MatchingReason = o?.matchingReason ?? string.Empty,
                            ConfidenceScore = (int)(o?.confidenceScore ?? 80)
                        });
                    }
                }
                return list;
            }
            catch
            {
                return new List<OutfitSuggestionDto>();
            }
        }

        private string BuildUserAnalysisPrompt(List<Outfit> outfits, List<Item> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Phân tích sở thích thời trang của người dùng.");
            sb.AppendLine($"Outfits: {outfits.Count}, Items: {items.Count}");
            return sb.ToString();
        }

        private string ExtractAnalysisFromResponse(string aiText)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(aiText);
                return data?.analysis ?? "";
            }
            catch { return string.Empty; }
        }

        public async Task<OutfitAIResponseDto> GenerateOutfitFromWardrobeAsync(OutfitAIRequestDto request)
        {
            try
            {
                // 1. Lấy tất cả items của user từ tủ đồ
                var userItemsFromService = await _itemService.GetItemsByUserIdAsync(request.UserId);
                var userItems = userItemsFromService.Select(item => new OutfitItemDto
                {
                    ItemId = item.Id,
                    ItemName = item.Name,
                    ItemImageUrl = item.FileUrl,
                    CategoryName = item.CategoryName ?? "",
                    ColorName = item.ColorName,
                    Brand = item.Brand,
                    Size = item.Size,
                    ItemType = item.CategoryName ?? ""
                }).ToList();

                if (!userItems.Any())
                {
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Bạn chưa có items nào trong tủ đồ. Vui lòng thêm items trước khi sử dụng tính năng này."
                    };
                }

                // 2. Lấy thông tin user để cá nhân hóa
                var user = await Repo.GetOneAsync<User>(u => u.Id == request.UserId);

                // 3. Tạo prompt cho Gemini AI
                var prompt = BuildWardrobeOutfitPrompt(request, userItems, user);

                // 4. Gọi Gemini AI để phân tích và đề xuất
                var aiResponse = await CallGeminiApiAsync(prompt);

                // 5. Parse response từ Gemini AI
                var selectedItems = ParseGeminiWardrobeResponse(aiResponse, userItems);

                if (!selectedItems.Any())
                {
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Không thể tạo outfit phù hợp từ các items hiện có. Vui lòng thử lại với yêu cầu khác hoặc thêm items mới."
                    };
                }

                // 6. Tạo hình ảnh outfit (placeholder for now)
                var imageUrl = await GenerateWardrobeOutfitImageAsync(selectedItems);

                // 7. Lưu outfit vào database
                var outfitId = await SaveGeneratedOutfitAsync(
                    selectedItems,
                    request.UserId,
                    $"Gemini AI Generated: {request.UserMessage}",
                    request.Occasion,
                    request.WeatherCondition,
                    request.Season
                );

                return new OutfitAIResponseDto
                {
                    Success = true,
                    Message = "Đã tạo outfit từ tủ đồ của bạn bằng Gemini AI!",
                    ImageUrl = imageUrl,
                    GeneratedOutfitId = outfitId,
                    OutfitItems = selectedItems,
                    RecommendationReason = ExtractGeminiRecommendationReason(aiResponse)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit from wardrobe using Gemini for user {UserId}", request.UserId);
                return new OutfitAIResponseDto
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tạo outfit bằng Gemini AI. Vui lòng thử lại sau."
                };
            }
        }

        private string BuildWardrobeOutfitPrompt(OutfitAIRequestDto request, List<OutfitItemDto> userItems, User user)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("Bạn là một stylist chuyên nghiệp sử dụng AI Gemini. Nhiệm vụ của bạn là tạo outfit hoàn hảo từ các items có sẵn trong tủ đồ của khách hàng.");
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
            if (!string.IsNullOrEmpty(request.Occasion))
                prompt.AppendLine($"- Dịp: {request.Occasion}");
            if (!string.IsNullOrEmpty(request.WeatherCondition))
                prompt.AppendLine($"- Thời tiết: {request.WeatherCondition}");
            if (!string.IsNullOrEmpty(request.Season))
                prompt.AppendLine($"- Mùa: {request.Season}");
            if (!string.IsNullOrEmpty(request.AdditionalPreferences))
                prompt.AppendLine($"- Ghi chú thêm: {request.AdditionalPreferences}");
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

        private async Task<Guid> SaveGeneratedOutfitAsync(List<OutfitItemDto> outfitItems, Guid userId, string description, string? occasion = null, string? weather = null, string? season = null)
        {
            var outfit = new Outfit
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = $"Gemini AI Outfit {DateTime.Now:dd/MM/yyyy HH:mm}",
                Description = description,
                Season = season,
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
                       "Gemini AI đã tạo outfit dựa trên phân tích các items trong tủ đồ và sở thích của bạn.";
            }
            catch
            {
                return "Gemini AI đã tạo outfit dựa trên phân tích các items trong tủ đồ và sở thích của bạn.";
            }
        }
    }
}


