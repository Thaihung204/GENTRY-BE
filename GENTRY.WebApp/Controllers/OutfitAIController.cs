using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.ChatDTOs;
using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OutfitAIController : BaseController
    {
        private readonly ILogger<OutfitAIController> _logger;
        private readonly IGeminiAIService _geminiAIService;
        private readonly ILoginService _loginService;

        public OutfitAIController(
            ILogger<OutfitAIController> logger,
            IExceptionHandler exceptionHandler,
            IGeminiAIService geminiAIService,
            ILoginService loginService) : base(exceptionHandler)
        {
            _logger = logger;
            _geminiAIService = geminiAIService;
            _loginService = loginService;
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

                // Lấy userId từ authentication context
                var user = await _loginService.GetCurrentUserAsync();
                if (user == null)
                {
                    return Unauthorized(new OutfitAIResponseDto
                    {
                        Success = false,
                        Message = "Bạn cần đăng nhập để sử dụng tính năng này."
                    });
                }

                var response = await _geminiAIService.GenerateOutfitFromWardrobeAsync(request, user.Id);

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
                var user = await _loginService.GetCurrentUserAsync();
                _logger.LogError(ex, "Error in GenerateOutfitRecommendationWithGemini for user {UserId}", user?.Id);
                return StatusCode(500, new OutfitAIResponseDto
                {
                    Success = false,
                    Message = "Có lỗi hệ thống xảy ra khi sử dụng GENTRY AI. Vui lòng thử lại sau."
                });
            }
        }

        /// <summary>
        /// Lấy lịch sử chat của người dùng với AI
        /// </summary>
        /// <returns>Danh sách chat messages</returns>
        [HttpGet("chat-history")]
        public async Task<ActionResult<List<ChatDto>>> GetChatHistory()
        {
            try
            {
                // Lấy userId từ authentication context
                var user = await _loginService.GetCurrentUserAsync();
                if (user == null)
                {
                    return Unauthorized(new { Success = false, Message = "Bạn cần đăng nhập để xem lịch sử chat." });
                }

                var chatHistory = await _geminiAIService.GetChatHistoryAsync(user.Id);
                
                // Map AIChatMessage to ChatDto
                var chatDtos = chatHistory.Select(chat => new ChatDto
                {
                    UserId = chat.UserId,
                    UserMessage = chat.UserMessage,
                    AIResponse = chat.AIResponse,
                    CreatedAt = chat.CreatedAt,
                    IsFromUser = chat.IsFromUser,
                    AdditionalContext = chat.AdditionalContext
                }).ToList();
                
                return Ok(new { Success = true, Data = chatDtos });
            }
            catch (Exception ex)
            {
                var user = await _loginService.GetCurrentUserAsync();
                _logger.LogError(ex, "Error retrieving chat history for user {UserId}", user?.Id);
                return StatusCode(500, new { Success = false, Message = "Có lỗi hệ thống xảy ra khi lấy lịch sử chat." });
            }
        }

    }
        
}