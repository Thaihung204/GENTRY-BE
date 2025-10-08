using GENTRY.WebApp.Services.DataTransferObjects.WeatherDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<List<WeatherDto>> GetAllAsync();
        Task<WeatherDto?> GetByIdAsync(int id);
    }
}
