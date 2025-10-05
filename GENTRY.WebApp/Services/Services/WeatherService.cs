using AutoMapper;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.WeatherDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class WeatherService : BaseService, IWeatherService
    {
        private readonly IMapper _mapper;

        public WeatherService(IRepository repo, IMapper mapper) : base(repo)
        {
            _mapper = mapper;
        }

        public async Task<List<WeatherDto>> GetAllAsync()
        {
            try
            {
                var weathers = await Repo.GetAsync<Weather>();
                return _mapper.Map<List<WeatherDto>>(weathers.ToList());
            }
            catch
            {
                return new List<WeatherDto>();
            }
        }

        public async Task<WeatherDto?> GetByIdAsync(int id)
        {
            try
            {
                var weather = await Repo.GetByIdAsync<Weather>(id);
                return weather != null ? _mapper.Map<WeatherDto>(weather) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
