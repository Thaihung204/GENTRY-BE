using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.FeedbackDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : BaseController
    {
        private readonly IRepository _repository;
        private readonly ILoginService _loginService;

        public FeedbacksController(
            IExceptionHandler exceptionHandler,
            IRepository repository,
            ILoginService loginService) 
            : base(exceptionHandler)
        {
            _repository = repository;
            _loginService = loginService;
        }

        /// <summary>
        /// Gửi feedback (không yêu cầu đăng nhập)
        /// POST: api/feedbacks
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ", Errors = ModelState });
                }

                // Lấy UserId nếu đã đăng nhập (optional)
                var currentUser = await _loginService.GetCurrentUserAsync();
                var userId = currentUser?.Id;

                var feedback = new Feedback
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = null, // Email không còn trong request
                    Rating = request.Rating,
                    Content = request.Content,
                    UserId = userId,
                    IsVisible = true,
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.CreateAsync(feedback);
                await _repository.SaveAsync();

                return Ok(new 
                { 
                    Success = true, 
                    Message = "Cảm ơn bạn đã gửi feedback!",
                    Data = new
                    {
                        feedback.Id,
                        feedback.Name,
                        feedback.Rating,
                        feedback.CreatedDate
                    }
                });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi gửi feedback");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi gửi feedback" });
            }
        }

        /// <summary>
        /// Lấy danh sách feedbacks công khai (chỉ hiển thị feedbacks được approve)
        /// GET: api/feedbacks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFeedbacks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Lấy tổng số feedbacks visible
                var totalCount = await _repository.GetCountAsync<Feedback>(f => f.IsVisible);
                
                // Lấy feedbacks với pagination
                var feedbacks = await _repository.GetAsync<Feedback>(
                    filter: f => f.IsVisible,
                    orderBy: q => q.OrderByDescending(f => f.CreatedDate),
                    skip: (page - 1) * pageSize,
                    take: pageSize
                );

                var feedbackDtos = feedbacks.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Email = f.Email,
                    Rating = f.Rating,
                    Content = f.Content,
                    UserId = f.UserId,
                    IsVisible = f.IsVisible,
                    CreatedDate = f.CreatedDate
                }).ToList();

                return Ok(new 
                { 
                    Success = true, 
                    Data = feedbackDtos,
                    Total = totalCount,
                    Page = page,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy danh sách feedback");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy danh sách feedback" });
            }
        }

        /// <summary>
        /// Lấy tất cả feedbacks (chỉ admin)
        /// GET: api/feedbacks/all
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFeedbacks([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                // Lấy tổng số feedbacks
                var totalCount = await _repository.GetCountAsync<Feedback>();
                
                // Lấy feedbacks với pagination
                var feedbacks = await _repository.GetAllAsync<Feedback>(
                    orderBy: q => q.OrderByDescending(f => f.CreatedDate),
                    skip: (page - 1) * pageSize,
                    take: pageSize
                );

                var feedbackDtos = feedbacks.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Email = f.Email,
                    Rating = f.Rating,
                    Content = f.Content,
                    UserId = f.UserId,
                    IsVisible = f.IsVisible,
                    CreatedDate = f.CreatedDate
                }).ToList();

                return Ok(new 
                { 
                    Success = true, 
                    Data = feedbackDtos,
                    Total = totalCount,
                    Page = page,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy danh sách feedback");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy danh sách feedback" });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hiển thị của feedback (chỉ admin)
        /// PATCH: api/feedbacks/{id}/visibility
        /// </summary>
        [HttpPatch("{id}/visibility")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleVisibility(Guid id, [FromBody] bool isVisible)
        {
            try
            {
                var feedback = await _repository.GetByIdAsync<Feedback>(id);
                if (feedback == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy feedback" });
                }

                feedback.IsVisible = isVisible;
                feedback.ModifiedDate = DateTime.UtcNow;

                _repository.Update(feedback);
                await _repository.SaveAsync();

                return Ok(new { Success = true, Message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi cập nhật feedback");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật feedback" });
            }
        }

        /// <summary>
        /// Xóa feedback (chỉ admin)
        /// DELETE: api/feedbacks/{id}
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            try
            {
                var feedback = await _repository.GetByIdAsync<Feedback>(id);
                if (feedback == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy feedback" });
                }

                _repository.Delete(feedback);
                await _repository.SaveAsync();

                return Ok(new { Success = true, Message = "Xóa feedback thành công" });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi xóa feedback");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa feedback" });
            }
        }

        /// <summary>
        /// Lấy thống kê feedbacks (chỉ admin)
        /// GET: api/feedbacks/statistics
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var feedbacks = (await _repository.GetAllAsync<Feedback>()).ToList();
                
                var statistics = new
                {
                    Total = feedbacks.Count,
                    Visible = feedbacks.Count(f => f.IsVisible),
                    Hidden = feedbacks.Count(f => !f.IsVisible),
                    AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0,
                    RatingDistribution = new
                    {
                        OneStar = feedbacks.Count(f => f.Rating == 1),
                        TwoStars = feedbacks.Count(f => f.Rating == 2),
                        ThreeStars = feedbacks.Count(f => f.Rating == 3),
                        FourStars = feedbacks.Count(f => f.Rating == 4),
                        FiveStars = feedbacks.Count(f => f.Rating == 5)
                    }
                };

                return Ok(new { Success = true, Data = statistics });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy thống kê feedback");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy thống kê feedback" });
            }
        }
    }
}

