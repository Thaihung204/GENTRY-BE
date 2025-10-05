using GENTRY.WebApp.Services.DataTransferObjects.ItemDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class ItemsController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly ILoginService _loginService;

        public ItemsController(IExceptionHandler exceptionHandler, IItemService itemService, ILoginService loginService) 
            : base(exceptionHandler)
        {
            _itemService = itemService;
            _loginService = loginService;
        }

        /// <summary>
        /// Lấy tất cả items của người dùng hiện tại
        /// GET: api/items
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyItems()
        {
            try
            {
                // Lấy thông tin người dùng hiện tại
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại" });
                }

                // Lấy danh sách items của người dùng
                var items = await _itemService.GetMyItemsAsync();

                return Ok(new 
                { 
                    Success = true, 
                    Message = "Lấy danh sách items thành công",
                    Data = items,
                    Count = items.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Success = false, 
                    Message = "Lỗi máy chủ nội bộ",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Thêm item mới
        /// POST: api/items
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] AddItemRequest request)
        {
            try
            {
                // Kiểm tra validation
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                    return BadRequest(new 
                    { 
                        Success = false, 
                        Message = "Dữ liệu không hợp lệ",
                        Errors = errors
                    });
                }

                // Lấy thông tin người dùng hiện tại
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại" });
                }

                // Thêm item mới
                var newItem = await _itemService.AddItemAsync(request);

                return CreatedAtAction(nameof(GetMyItems), new 
                { 
                    Success = true, 
                    Message = "Thêm trang phục thành công",
                    Data = newItem
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Success = false, 
                    Message = "Lỗi máy chủ nội bộ",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy tất cả items của một người dùng cụ thể (cho admin)
        /// GET: api/items/user/{userId}
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetItemsByUserId(Guid userId)
        {
            try
            {
                // Kiểm tra quyền truy cập (có thể cần thêm logic kiểm tra admin)
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại" });
                }

                // Chỉ cho phép lấy items của chính mình hoặc nếu là admin
                if (currentUser.Id != userId && currentUser.Role != "Admin")
                {
                    return Forbid("Bạn không có quyền truy cập dữ liệu này");
                }

                // Lấy danh sách items của người dùng
                var items = await _itemService.GetItemsByUserIdAsync(userId);

                return Ok(new 
                { 
                    Success = true, 
                    Message = "Lấy danh sách items thành công",
                    Data = items,
                    Count = items.Count
                });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new 
                { 
                    Success = false, 
                    Message = "Lỗi máy chủ nội bộ",
                    Error = ex.Message
                });
            }
        }
    }
}
