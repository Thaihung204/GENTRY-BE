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
}