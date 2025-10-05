using AutoMapper;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.UserDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        
        public UsersController(IExceptionHandler exceptionHandler, IUserService userService, ILoginService loginService, IMapper mapper) : base(exceptionHandler)
        {
            _userService = userService;
            _loginService = loginService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                var userDtos = _mapper.Map<List<UserDto>>(users);
                return Ok(new { Success = true, Data = userDtos });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy danh sách người dùng");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy danh sách người dùng" });
            }
        }

        [HttpGet("profile")]
        [Authorize] 
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var currentUser = await _loginService.GetCurrentUserAsync();
                
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại" });
                }

                var userProfileDto = _mapper.Map<UserProfileDto>(currentUser);
                return Ok(new { Success = true, Data = userProfileDto });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy thông tin profile user");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy thông tin profile" });
            }
        }

        [HttpPut("profile")]
        [Authorize] 
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ", Errors = ModelState });
                }
                var currentUser = await _loginService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { Success = false, Message = "Người dùng chưa đăng nhập hoặc không tồn tại" });
                }
                _mapper.Map(updateDto, currentUser);
                await _userService.UpdateUserAsync(currentUser);
                var updatedUser = await _loginService.GetCurrentUserAsync();
                var userProfileDto = _mapper.Map<UserProfileDto>(updatedUser);

                return Ok(new { Success = true, Message = "Cập nhật profile thành công", Data = userProfileDto });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi cập nhật profile user");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật profile" });
            }
        }

        [HttpGet("{userId}/profile")]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(new { Success = false, Message = "User ID không hợp lệ" });
                }

                var user = await _userService.GetUserProfileAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy người dùng" });
                }

                var userProfileDto = _mapper.Map<UserProfileDto>(user);
                return Ok(new { Success = true, Data = userProfileDto });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy thông tin profile user");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy thông tin profile" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AddUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ", Errors = ModelState });
                }

                var user = _mapper.Map<User>(request);
                var createdUser = await _userService.CreateUserAsync(user);
                var userDto = _mapper.Map<UserDto>(createdUser);

                return CreatedAtAction(nameof(GetUserProfile), new { userId = createdUser.Id }, 
                    new { Success = true, Message = "Tạo người dùng thành công", Data = userDto });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi tạo người dùng");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo người dùng" });
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ", Errors = ModelState });
                }

                if (userId == Guid.Empty)
                {
                    return BadRequest(new { Success = false, Message = "User ID không hợp lệ" });
                }

                // Get existing user first
                var existingUser = await _userService.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy người dùng" });
                }

                // Map the request to existing user, preserving existing values for null properties
                _mapper.Map(request, existingUser);
                
                var updatedUser = await _userService.UpdateUserAsync(existingUser, userId);

                if (updatedUser == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy người dùng" });
                }

                var userDto = _mapper.Map<UserDto>(updatedUser);
                return Ok(new { Success = true, Message = "Cập nhật người dùng thành công", Data = userDto });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi cập nhật người dùng");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật người dùng" });
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(new { Success = false, Message = "User ID không hợp lệ" });
                }

                var deletedUser = await _userService.DeleteUserAsync(userId);

                if (deletedUser == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy người dùng" });
                }

                return Ok(new { Success = true, Message = "Xóa người dùng thành công" });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi xóa người dùng");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa người dùng" });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(new { Success = false, Message = "User ID không hợp lệ" });
                }

                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy người dùng" });
                }

                var userDto = _mapper.Map<UserDto>(user);
                return Ok(new { Success = true, Data = userDto });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Lỗi khi lấy thông tin người dùng");
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi lấy thông tin người dùng" });
            }
        }
    }
}
