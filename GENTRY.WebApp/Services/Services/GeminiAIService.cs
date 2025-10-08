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
        private readonly string _geminiApiKey;
        private readonly string _geminiEndpoint;

        public GeminiAIService(
            IRepository repository,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiAIService> logger,
            IAffiliateService affiliateService) : base(repository, httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _affiliateService = affiliateService;

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
    }
}


