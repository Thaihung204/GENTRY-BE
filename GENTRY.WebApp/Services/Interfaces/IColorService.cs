using GENTRY.WebApp.Models;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IColorService
    {
        Task<List<Color>> GetAllAsync();
    }
}
