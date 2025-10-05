using AutoMapper;
using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class StyleService : BaseService, IStyleService
    {
        private readonly IMapper _mapper;

        public StyleService(IRepository repo, IMapper mapper) : base(repo)
        {
            _mapper = mapper;
        }

        public async Task<List<Style>> GetAllAsync()
        {
            try
            {
                var styles = await Repo.GetAsync<Style>();
                return styles.ToList();
            }
            catch
            {
                return new List<Style>();
            }
        }

        public async Task<Style?> GetByIdAsync(int id)
        {
            try
            {
                var style = await Repo.GetByIdAsync<Style>(id);
                return style;
            }
            catch
            {
                return null;
            }
        }
    }
}
