using GENTRY.WebApp.Services.Interfaces;
using GENTRY.WebApp.Models;

namespace GENTRY.WebApp.Services.Services
{
    public class ColorService : BaseService, IColorService
    {
        public ColorService(IRepository repo) : base(repo)
        {
        }

        public async Task<List<Color>> GetAllAsync()
        {
            try
            {
                var colors = await Repo.GetAsync<Color>(
                    filter: c => c.IsActive == true,
                    orderBy: q => q.OrderBy(c => c.Name)
                );

                return colors.ToList();
            }
            catch
            {
                return new List<Color>();
            }
        }
    }
}
