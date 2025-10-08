using GENTRY.WebApp.Services.DataTransferObjects.OccasionDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OccasionsController : BaseController
    {
        private readonly IOccasionService _occasionService;

        public OccasionsController(IExceptionHandler exceptionHandler, IOccasionService occasionService) : base(exceptionHandler)
        {
            _occasionService = occasionService;
        }

        /// <summary>
        /// Get all occasions
        /// </summary>
        /// <returns>List of occasions</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOccasions()
        {
            var occasions = await _occasionService.GetAllAsync();
            return Ok(occasions);
        }

        /// <summary>
        /// Get occasion by ID
        /// </summary>
        /// <param name="id">Occasion ID</param>
        /// <returns>Occasion details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOccasionById(int id)
        {
            var occasion = await _occasionService.GetByIdAsync(id);
            if (occasion == null)
            {
                return NotFound($"Occasion with ID {id} not found.");
            }

            return Ok(occasion);
        }
    }
}
