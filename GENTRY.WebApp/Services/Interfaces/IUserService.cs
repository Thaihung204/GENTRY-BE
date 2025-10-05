using GENTRY.WebApp.Models;

namespace GENTRY.WebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(User user, Guid id);
        Task<User?> DeleteUserAsync(Guid id);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserProfileAsync(Guid userId);
        Task UpdateUserAsync(User user);
    }
}
