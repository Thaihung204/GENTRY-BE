using GENTRY.WebApp.Models;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IStyleService
    {
        Task<List<Style>> GetAllAsync();
        Task<Style?> GetByIdAsync(int id);
    }
}   
