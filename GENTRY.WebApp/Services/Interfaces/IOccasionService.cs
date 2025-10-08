using GENTRY.WebApp.Services.DataTransferObjects.OccasionDTOs;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IOccasionService
    {
        Task<List<OccasionDto>> GetAllAsync();
        Task<OccasionDto?> GetByIdAsync(int id);
    }
}
