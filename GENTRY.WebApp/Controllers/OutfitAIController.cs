using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutfitAIController : BaseController
    {
        private readonly IOutfitAIService _outfitAIService;
        private readonly ILogger<OutfitAIController> _logger;

        public OutfitAIController(
            IOutfitAIService outfitAIService,
            ILogger<OutfitAIController> logger,
            IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            _outfitAIService = outfitAIService;
            _logger = logger;
        }

        /// <summary>
        /// Chatbot endpoint - Tạo outfit recommendation từ yêu cầu của user
        /// </summary>
        /// <param name="request">Yêu cầu từ user qua chatbot</param>
        /// <returns>Outfit recommendation với hình ảnh</returns>
        [HttpPost("chat")]
        public async Task<ActionResult<OutfitAIResponseDto>> GenerateOutfitRecommendation([FromBody] OutfitAIRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Dữ liệu đầu vào không hợp lệ."
                    });
                }

                var response = await _outfitAIService.GenerateOutfitRecommendationAsync(request);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateOutfitRecommendation for user {UserId}", request.UserId);
                return StatusCode(500, new OutfitAIResponseDto
                {
                    Success = false,
                    Message = "Có lỗi hệ thống xảy ra. Vui lòng thử lại sau."
                });
            }
        }


        /// <summary>
        /// AI Styling - Gợi ý outfit dựa trên filters và trả về affiliate products
        /// </summary>
        [HttpPost("ai-styling")]
        public async Task<ActionResult<AiStylingResponseDto>> GenerateAIStyling([FromBody] AiStylingRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AiStylingResponseDto
                    {
                        Success = false,
                        Message = "Dữ liệu đầu vào không hợp lệ."
                    });
                }

                var gemini = HttpContext.RequestServices.GetRequiredService<IGeminiAIService>();
                var response = await gemini.GenerateOutfitSuggestionsAsync(request);
                if (response.Success) return Ok(response);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AI Styling for user {UserId}", request.UserId);
                return StatusCode(500, new AiStylingResponseDto
                {
                    Success = false,
                    Message = "Có lỗi hệ thống xảy ra. Vui lòng thử lại sau."
                });
            }
        }



        /// <summary>
        /// Tạo hình ảnh outfit từ danh sách items đã chọn
        /// </summary>
        /// <param name="request">Danh sách items và user ID</param>
        /// <returns>URL của hình ảnh outfit</returns>
        [HttpPost("generate-image")]
        public async Task<ActionResult<string>> GenerateOutfitImage([FromBody] GenerateImageRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Dữ liệu đầu vào không hợp lệ.");
                }

                var imageUrl = await _outfitAIService.GenerateOutfitImageAsync(request.OutfitItems, request.UserId);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    return Ok(new { imageUrl = imageUrl });
                }
                else
                {
                    return BadRequest(new { message = "Không thể tạo hình ảnh outfit." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating outfit image for user {UserId}", request.UserId);
                return StatusCode(500, new { message = "Có lỗi xảy ra khi tạo hình ảnh." });
            }
        }

        /// <summary>
        /// Endpoint để test kết nối Gemini AI
        /// </summary>
        /// <returns>Status của Gemini API connection</returns>
        [HttpGet("test-gemini")]
        public async Task<ActionResult> TestGeminiConnection()
        {
            try
            {
                var geminiService = HttpContext.RequestServices.GetRequiredService<IGeminiAIService>();
                
                // Test với một câu hỏi đơn giản
                var testUserId = Guid.NewGuid();
                var testAnalysis = await geminiService.AnalyzeUserPreferencesAsync(testUserId);
                
                return Ok(new
                {
                    status = "success",
                    service = "Gemini AI",
                    timestamp = DateTime.UtcNow,
                    message = "Gemini API đang hoạt động bình thường",
                    testResult = testAnalysis,
                    apiConnected = true
                });
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Gemini API connection failed");
                return StatusCode(502, new 
                { 
                    status = "failed", 
                    service = "Gemini AI",
                    message = "Không thể kết nối tới Gemini API. Kiểm tra API key và network.",
                    error = httpEx.Message,
                    apiConnected = false
                });
            }
            catch (InvalidOperationException configEx)
            {
                _logger.LogError(configEx, "Gemini configuration error");
                return StatusCode(500, new 
                { 
                    status = "failed", 
                    service = "Gemini AI",
                    message = "Lỗi cấu hình Gemini API key. Kiểm tra appsettings.json.",
                    error = configEx.Message,
                    apiConnected = false
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini test failed");
                return StatusCode(500, new 
                { 
                    status = "failed", 
                    service = "Gemini AI",
                    message = "Lỗi không xác định khi test Gemini API.",
                    error = ex.Message,
                    apiConnected = false
                });
            }
        }

        /// <summary>
        /// Test Gemini với prompt custom
        /// </summary>
        /// <param name="prompt">Test prompt</param>
        /// <returns>Gemini response</returns>
        [HttpPost("test-gemini-prompt")]
        public async Task<ActionResult> TestGeminiWithPrompt([FromBody] TestGeminiRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Prompt))
                {
                    return BadRequest(new { message = "Prompt không được để trống" });
                }

                var geminiService = HttpContext.RequestServices.GetRequiredService<IGeminiAIService>();
                
                // Gọi trực tiếp CallGeminiApiAsync thông qua reflection (cho test)
                var geminiServiceType = geminiService.GetType();
                var callGeminiMethod = geminiServiceType.GetMethod("CallGeminiApiAsync", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (callGeminiMethod != null)
                {
                    var task = (Task<string>)callGeminiMethod.Invoke(geminiService, new object[] { request.Prompt });
                    var response = await task;
                    
                    return Ok(new
                    {
                        status = "success",
                        prompt = request.Prompt,
                        response = response,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return StatusCode(500, new { message = "Không thể access CallGeminiApiAsync method" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Custom Gemini prompt test failed");
                return StatusCode(500, new 
                { 
                    status = "failed",
                    message = "Lỗi khi test custom prompt",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Endpoint để test kết nối AI (development only)
        /// </summary>
        /// <returns>Status của AI service</returns>
        [HttpGet("health")]
        public ActionResult GetHealthStatus()
        {
            try
            {
                return Ok(new
                {
                    status = "healthy",
                    service = "OutfitAI",
                    timestamp = DateTime.UtcNow,
                    message = "AI Outfit service đang hoạt động bình thường."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(500, new { status = "unhealthy", message = "AI service gặp sự cố." });
            }
        }
    }

    public class TestGeminiRequest
    {
        public string Prompt { get; set; } = null!;
    }
}