using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenAI.Chat;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace GENTRY.WebApp.Services.Services
{
    public class OutfitAIService : BaseService, IOutfitAIService
    {
        private readonly ChatClient _chatClient;
        private readonly IFileService _fileService;
        private readonly ILogger<OutfitAIService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemService _itemService;

        public OutfitAIService(
            IRepository repository, 
            IHttpContextAccessor httpContextAccessor,
            IFileService fileService,
            ILogger<OutfitAIService> logger,
            IConfiguration configuration,
            IItemService itemService) : base(repository, httpContextAccessor)
        {
            _configuration = configuration;
            _itemService = itemService;
            
            // Khởi tạo OpenAI client - cần configuration
            var apiKey = _configuration["OpenAI:ApiKey"] ?? 
                         Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? 
                         throw new InvalidOperationException("OpenAI API key not configured in appsettings.json or environment variables");
            
            var model = _configuration["OpenAI:Model"] ?? "gpt-4o";
            _chatClient = new ChatClient(model, apiKey);
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<OutfitAIResponseDto> GenerateOutfitRecommendationAsync(OutfitAIRequestDto request)
        {
            try
            {
                // 1. Lấy tất cả items của user thông qua ItemService
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
                
                // 3. Tạo prompt cho AI
                var prompt = BuildAIPrompt(request, userItems, user);
                
                // 4. Gọi AI để phân tích và đề xuất
                var aiResponse = await CallOpenAIAsync(prompt);
                
                // 5. Parse response từ AI
                var selectedItems = ParseAIResponse(aiResponse, userItems);
                
                if (!selectedItems.Any())
                {
                    return new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Không thể tạo outfit phù hợp từ các items hiện có. Vui lòng thử lại với yêu cầu khác."
                    };
                }

                // 6. Tạo hình ảnh outfit
                var imageUrl = await GenerateOutfitImageAsync(selectedItems, request.UserId);
                
                // 7. Lưu outfit vào database
                var outfitId = await SaveGeneratedOutfitAsync(
                    selectedItems, 
                    request.UserId, 
                    $"AI Generated Outfit: {request.UserMessage}",
                    request.Occasion,
                    request.WeatherCondition,
                    request.Season
                );

                return new OutfitAIResponseDto
                {
                    Success = true,
                    Message = "Đã tạo outfit phù hợp với yêu cầu của bạn!",
                    ImageUrl = imageUrl,
                    GeneratedOutfitId = outfitId,
                    OutfitItems = selectedItems,
                    RecommendationReason = ExtractRecommendationReason(aiResponse)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit recommendation for user {UserId}", request.UserId);
                return new OutfitAIResponseDto
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tạo outfit. Vui lòng thử lại sau."
                };
            }
        }



        public async Task<string?> GenerateOutfitImageAsync(List<OutfitItemDto> outfitItems, Guid userId)
        {
            try
            {
                // Tạo collage từ các hình ảnh items
                // Tạo logger factory để tạo logger cho OutfitImageGenerator
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var imageGenerator = new OutfitImageGenerator(loggerFactory.CreateLogger<OutfitImageGenerator>());
                
                // Tạo layout outfit
                var imageBytes = await imageGenerator.CreateOutfitLayoutAsync(outfitItems);
                
                if (imageBytes != null)
                {
                    // Upload hình ảnh lên cloud storage
                    using var stream = new MemoryStream(imageBytes);
                    // Tạo IFormFile từ stream để upload
                    var formFile = new FormFile(stream, 0, stream.Length, "outfit", $"outfit_{userId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jpg")
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/jpeg"
                    };
                    
                    var uploadResult = await _fileService.UploadImageAsync(formFile, "outfits");
                    
                    if (uploadResult != null)
                    {
                        return uploadResult.Url;
                    }
                }
                
                // Fallback: return URL của item đầu tiên nếu không tạo được collage
                var firstItemWithImage = outfitItems.FirstOrDefault(i => !string.IsNullOrEmpty(i.ItemImageUrl));
                return firstItemWithImage?.ItemImageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit image for user {UserId}", userId);
                
                // Fallback: return URL của item đầu tiên
                var firstItemWithImage = outfitItems.FirstOrDefault(i => !string.IsNullOrEmpty(i.ItemImageUrl));
                return firstItemWithImage?.ItemImageUrl;
            }
        }

        public async Task<Guid> SaveGeneratedOutfitAsync(List<OutfitItemDto> outfitItems, Guid userId, string description, string? occasion = null, string? weather = null, string? season = null)
        {
            var outfit = new Outfit
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = $"AI Outfit {DateTime.Now:dd/MM/yyyy HH:mm}",
                Description = description,
                Occasion = occasion,
                WeatherCondition = weather,
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

        private string BuildAIPrompt(OutfitAIRequestDto request, List<OutfitItemDto> userItems, User user)
        {
            var prompt = new StringBuilder();
            
            prompt.AppendLine("Bạn là một stylist chuyên nghiệp. Nhiệm vụ của bạn là tạo outfit từ các items có sẵn trong tủ đồ của khách hàng.");
            prompt.AppendLine();
            
            // Thông tin khách hàng
            prompt.AppendLine("THÔNG TIN KHÁCH HÀNG:");
            prompt.AppendLine($"- Giới tính: {user.Gender ?? "Không xác định"}");
            prompt.AppendLine($"- Tuổi: {(user.BirthDate.HasValue ? DateTime.Now.Year - user.BirthDate.Value.Year : "Không xác định")}");
            prompt.AppendLine($"- Sở thích style: {user.StylePreferences ?? "Không có"}");
            prompt.AppendLine($"- Body type: {user.BodyType ?? "Không xác định"}");
            prompt.AppendLine($"- Skin tone: {user.SkinTone ?? "Không xác định"}");
            prompt.AppendLine();

            // Yêu cầu cụ thể
            prompt.AppendLine("YÊU CẦU CỦA KHÁCH HÀNG:");
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

            // Danh sách items
            prompt.AppendLine("CÁC ITEMS CÓ SẴN TRONG TỦ ĐỒ:");
            foreach (var item in userItems)
            {
                prompt.AppendLine($"- ID: {item.ItemId}, Tên: {item.ItemName}, Loại: {item.CategoryName}, Màu: {item.ColorName ?? "N/A"}, Brand: {item.Brand ?? "N/A"}, Size: {item.Size ?? "N/A"}");
            }
            prompt.AppendLine();

            prompt.AppendLine("NHIỆM VỤ:");
            prompt.AppendLine("1. Phân tích yêu cầu và thông tin khách hàng");
            prompt.AppendLine("2. Chọn các items phù hợp từ danh sách có sẵn");
            prompt.AppendLine("3. Tạo outfit hoàn chỉnh và hài hòa");
            prompt.AppendLine("4. Giải thích lý do lựa chọn");
            prompt.AppendLine();

            prompt.AppendLine("ĐỊNH DẠNG PHẢN HỒI (JSON):");
            prompt.AppendLine("{");
            prompt.AppendLine("  \"selectedItems\": [");
            prompt.AppendLine("    {\"itemId\": \"guid\", \"reason\": \"lý do chọn item này\"}");
            prompt.AppendLine("  ],");
            prompt.AppendLine("  \"overallReason\": \"lý do tổng thể cho outfit này\",");
            prompt.AppendLine("  \"stylingTips\": \"gợi ý styling\"");
            prompt.AppendLine("}");

            return prompt.ToString();
        }

        private async Task<string> CallOpenAIAsync(string prompt)
        {
            try
            {
                var messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage("Bạn là một stylist chuyên nghiệp, trả lời bằng tiếng Việt và theo đúng format JSON được yêu cầu."),
                    ChatMessage.CreateUserMessage(prompt)
                };

                var completion = await _chatClient.CompleteChatAsync(messages);
                return completion.Value.Content[0].Text;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenAI API");
                throw new Exception("Không thể kết nối với AI service");
            }
        }

        private List<OutfitItemDto> ParseAIResponse(string aiResponse, List<OutfitItemDto> availableItems)
        {
            try
            {
                // Parse JSON response từ AI
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
                _logger.LogError(ex, "Error parsing AI response: {Response}", aiResponse);
                
                // Fallback: chọn random một số items nếu không parse được
                return availableItems.Take(3).ToList();
            }
        }

        private string ExtractRecommendationReason(string aiResponse)
        {
            try
            {
                var responseData = JsonConvert.DeserializeObject<dynamic>(aiResponse);
                return responseData?.overallReason?.ToString() ?? "AI đã tạo outfit dựa trên phân tích các items trong tủ đồ của bạn.";
            }
            catch
            {
                return "AI đã tạo outfit dựa trên phân tích các items trong tủ đồ của bạn.";
            }
        }
    }
} 