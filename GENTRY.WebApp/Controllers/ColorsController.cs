using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GENTRY.WebApp.Services.DataTransferObjects.ColorDTOs;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : BaseController
    {
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;

        public ColorsController(IExceptionHandler exceptionHandler, IColorService colorService, IMapper mapper) : base(exceptionHandler)
        {
            _colorService = colorService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy tất cả màu sắc
        /// GET: api/colors
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllColors()
        {
            try
            {
                var colors = await _colorService.GetAllAsync();
                var colorDtos = _mapper.Map<List<ColorDto>>(colors);

                return Ok(new 
                { 
                    Success = true, 
                    Message = "Lấy danh sách màu sắc thành công",
                    Data = colorDtos,
                    Count = colorDtos.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new 
                { 
                    Success = false, 
                    Message = "Lấy danh sách màu sắc thất bại", 
                    Error = ex.Message 
                });
            }
        }
    }
}
