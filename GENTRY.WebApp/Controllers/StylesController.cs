using AutoMapper;
using GENTRY.WebApp.Services.DataTransferObjects.StyleDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StylesController : BaseController
    {
        private readonly IStyleService _styleService;
        private readonly IMapper _mapper;

        public StylesController(IExceptionHandler exceptionHandler, IStyleService styleService, IMapper mapper) : base(exceptionHandler)
        {
            _styleService = styleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all styles
        /// </summary>
        /// <returns>List of styles</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStyles()
        {
            var styles = await _styleService.GetAllAsync();
            var styleDtos = _mapper.Map<List<StyleDto>>(styles);
            return Ok(styleDtos);
        }

        /// <summary>
        /// Get style by ID
        /// </summary>
        /// <param name="id">Style ID</param>
        /// <returns>Style details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStyleById(int id)
        {
            var style = await _styleService.GetByIdAsync(id);
            if (style == null)
            {
                return NotFound($"Style with ID {id} not found.");
            }

            var styleDto = _mapper.Map<StyleDto>(style);
            return Ok(styleDto);
        }
    }
}
