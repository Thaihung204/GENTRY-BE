using AutoMapper;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.DataTransferObjects.OccasionDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class OccasionService : BaseService, IOccasionService
    {
        private readonly IMapper _mapper;

        public OccasionService(IRepository repo, IMapper mapper) : base(repo)
        {
            _mapper = mapper;
        }

        public async Task<List<OccasionDto>> GetAllAsync()
        {
            try
            {
                var occasions = await Repo.GetAsync<Occasion>();
                return _mapper.Map<List<OccasionDto>>(occasions.ToList());
            }
            catch
            {
                return new List<OccasionDto>();
            }
        }

        public async Task<OccasionDto?> GetByIdAsync(int id)
        {
            try
            {
                var occasion = await Repo.GetByIdAsync<Occasion>(id);
                return occasion != null ? _mapper.Map<OccasionDto>(occasion) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
