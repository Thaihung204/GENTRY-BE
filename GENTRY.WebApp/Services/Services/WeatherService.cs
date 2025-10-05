using GENTRY.WebApp.Services.DataTransferObjects.WeatherDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly List<WeatherDto> _weatherData;

        public WeatherService()
        {
            // Initialize predefined weather conditions
            _weatherData = new List<WeatherDto>
            {
                new WeatherDto { Id = 1, Name = "Sunny", Description = "Nắng", Icon = "☀️", IsActive = true },
                new WeatherDto { Id = 2, Name = "Cloudy", Description = "Có mây", Icon = "☁️", IsActive = true },
                new WeatherDto { Id = 3, Name = "Rainy", Description = "Mưa", Icon = "🌧️", IsActive = true },
                new WeatherDto { Id = 4, Name = "Stormy", Description = "Bão", Icon = "⛈️", IsActive = true },
                new WeatherDto { Id = 5, Name = "Snowy", Description = "Tuyết", Icon = "❄️", IsActive = true },
                new WeatherDto { Id = 6, Name = "Windy", Description = "Có gió", Icon = "💨", IsActive = true },
                new WeatherDto { Id = 7, Name = "Hot", Description = "Nóng", Icon = "🔥", IsActive = true },
                new WeatherDto { Id = 8, Name = "Cold", Description = "Lạnh", Icon = "🧊", IsActive = true },
                new WeatherDto { Id = 9, Name = "Humid", Description = "Ẩm ướt", Icon = "💧", IsActive = true },
                new WeatherDto { Id = 10, Name = "Dry", Description = "Khô", Icon = "🌵", IsActive = true }
            };
        }

        public async Task<List<WeatherDto>> GetAllAsync()
        {
            await Task.Delay(1); // Simulate async operation
            return _weatherData.Where(w => w.IsActive).ToList();
        }

        public async Task<WeatherDto?> GetByIdAsync(int id)
        {
            await Task.Delay(1); // Simulate async operation
            return _weatherData.FirstOrDefault(w => w.Id == id && w.IsActive);
        }
    }
}
