using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutfitAIController : BaseController
    {
        private readonly ILogger<OutfitAIController> _logger;
        private readonly ILoginService _loginService;
        private readonly IChatHistoryService _chatHistoryService;

        public OutfitAIController(
            ILogger<OutfitAIController> logger,
            ILoginService loginService,
            IChatHistoryService chatHistoryService,
            IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            _logger = logger;
            _loginService = loginService;
            _chatHistoryService = chatHistoryService;
        }


        /// <summary>
        /// Chatbot endpoint với Gemini AI - Tạo outfit recommendation từ items có sẵn trong tủ đồ
        /// </summary>
        /// <param name="request">Yêu cầu từ user qua chatbot</param>
        /// <returns>Outfit recommendation từ tủ đồ sử dụng Gemini AI</returns>
        [HttpPost("chat")]
        public async Task<ActionResult<OutfitAIResponseDto>> GenerateOutfitRecommendationWithGemini([FromBody] OutfitAIRequestDto request)
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

                // Lấy thông tin user hiện tại từ authentication context
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Người dùng chưa đăng nhập hoặc không tồn tại."
                    });
                }

                var geminiService = HttpContext.RequestServices.GetRequiredService<IGeminiAIService>();
                var response = await geminiService.GenerateOutfitFromWardrobeAsync(request, currentUser.Id);

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
                _logger.LogError(ex, "Error in GenerateOutfitRecommendationWithGemini for user {UserId}", "Unknown");
                return StatusCode(500, new OutfitAIResponseDto
                {
                    Success = false,
                    Message = "Có lỗi hệ thống xảy ra khi sử dụng Gemini AI. Vui lòng thử lại sau."
                });
            }
        }


        /// <summary>
        /// Lấy lịch sử chat của người dùng hiện tại
        /// </summary>
        /// <param name="limit">Giới hạn số lượng tin nhắn (mặc định 20)</param>
        /// <returns>Danh sách lịch sử chat</returns>
        [HttpGet("chat-history")]
        public async Task<ActionResult> GetChatHistory([FromQuery] int limit = 20)
        {
            try
            {
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại." });
                }

                var chatHistory = await _chatHistoryService.GetChatHistoryAsync(currentUser.Id, limit);

                return Ok(new
                {
                    Success = true,
                    Message = "Lấy lịch sử chat thành công",
                    Data = chatHistory,
                    Count = chatHistory.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat history");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy lịch sử chat." });
            }
        }

        /// <summary>
        /// Xóa một tin nhắn chat cụ thể
        /// </summary>
        /// <param name="chatId">ID của chat cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("chat-history/{chatId}")]
        public async Task<ActionResult> DeleteChat(Guid chatId)
        {
            try
            {
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại." });
                }

                var success = await _chatHistoryService.DeleteChatAsync(chatId, currentUser.Id);

                if (success)
                {
                    return Ok(new { Success = true, Message = "Xóa tin nhắn thành công" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy tin nhắn hoặc bạn không có quyền xóa." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat {ChatId}", chatId);
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa tin nhắn." });
            }
        }

        /// <summary>
        /// Xóa toàn bộ lịch sử chat của người dùng hiện tại
        /// </summary>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("chat-history")]
        public async Task<ActionResult> ClearAllChatHistory()
        {
            try
            {
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại." });
                }

                var success = await _chatHistoryService.ClearAllChatHistoryAsync(currentUser.Id);

                if (success)
                {
                    return Ok(new { Success = true, Message = "Đã xóa toàn bộ lịch sử chat" });
                }
                else
                {
                    return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa lịch sử chat." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all chat history");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa lịch sử chat." });
            }
        }

        /// <summary>
        /// Test endpoint không cần authentication
        /// </summary>
        [HttpGet("test")]
        [AllowAnonymous]
        public ActionResult Test()
        {
            return Ok(new { 
                message = "OutfitAI Controller is working!", 
                timestamp = DateTime.UtcNow,
                status = "success"
            });
        }
    }

    public class TestGeminiRequest
    {
        public string Prompt { get; set; } = null!;
    }

    public class TestGeminiWardrobeRequest
    {
        public Guid UserId { get; set; }
        public string Message { get; set; } = null!;
        public string? Occasion { get; set; }
        public string? WeatherCondition { get; set; }
        public string? Season { get; set; }
        public string? AdditionalPreferences { get; set; }
    }
}