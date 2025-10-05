using GENTRY.WebApp.Services.DataTransferObjects.WeatherDTOs;
using GENTRY.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GENTRY.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeathersController : BaseController
    {
        private readonly IWeatherService _weatherService;

        public WeathersController(IExceptionHandler exceptionHandler, IWeatherService weatherService) : base(exceptionHandler)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Get all weather conditions
        /// </summary>
        /// <returns>List of weather conditions</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllWeathers()
        {
            var weathers = await _weatherService.GetAllAsync();
            return Ok(weathers);
        }

        /// <summary>
        /// Get weather condition by ID
        /// </summary>
        /// <param name="id">Weather ID</param>
        /// <returns>Weather condition details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherById(int id)
        {
            var weather = await _weatherService.GetByIdAsync(id);
            if (weather == null)
            {
                return NotFound($"Weather condition with ID {id} not found.");
            }

            return Ok(weather);
        }
    }
}
