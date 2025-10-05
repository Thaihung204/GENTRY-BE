using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace GENTRY.WebApp.Services.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IRepository repo) : base(repo)
        {
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            // Check if email already exists
            var existingUser = await Repo.GetOneAsync<User>(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email đã được sử dụng");
            }
            
            // Check if phone already exists (if provided)
            if (!string.IsNullOrEmpty(user.Phone))
            {
                var existingPhone = await Repo.GetOneAsync<User>(u => u.Phone == user.Phone);
                if (existingPhone != null)
                {
                    throw new InvalidOperationException("Số điện thoại đã được sử dụng");
                }
            }
            
            user.Id = Guid.NewGuid();
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;
            
            await Repo.CreateAsync(user);
            await Repo.SaveAsync();
            
            return user;
        }

        public async Task<User?> DeleteUserAsync(Guid id)
        {
            var user = await Repo.GetByIdAsync<User>(id);
            if (user == null || !user.IsActive)
            {
                return null;
            }
            // Soft delete by setting IsActive to false
            user.IsActive = false;
            user.ModifiedDate = DateTime.UtcNow;
            
            Repo.Update(user);
            await Repo.SaveAsync();
            
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await Repo.GetAsync<User>(
                    filter: u => u.IsActive == true,
                    orderBy: q => q.OrderBy(u => u.CreatedDate)
                );

                return users.ToList();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await Repo.GetByIdAsync<User>(userId);
                return user?.IsActive == true ? user : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var user = await Repo.GetByIdAsync<User>(userId);
                return user?.IsActive == true ? user : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.ModifiedDate = DateTime.UtcNow;
            Repo.Update(user);
            await Repo.SaveAsync();
        }

        public async Task<User?> UpdateUserAsync(User updateData, Guid id)
        {
            if (updateData == null) throw new ArgumentNullException(nameof(updateData));
            
            var existingUser = await Repo.GetByIdAsync<User>(id);
            if (existingUser == null || !existingUser.IsActive)
            {
                return null;
            }
            
            // Check email uniqueness if email is being changed
            if (!string.IsNullOrEmpty(updateData.Email) && updateData.Email != existingUser.Email)
            {
                var emailExists = await Repo.GetOneAsync<User>(u => u.Email == updateData.Email && u.Id != id);
                if (emailExists != null)
                {
                    throw new InvalidOperationException("Email đã được sử dụng");
                }
            }
            
            // Check phone uniqueness if phone is being changed
            if (!string.IsNullOrEmpty(updateData.Phone) && updateData.Phone != existingUser.Phone)
            {
                var phoneExists = await Repo.GetOneAsync<User>(u => u.Phone == updateData.Phone && u.Id != id);
                if (phoneExists != null)
                {
                    throw new InvalidOperationException("Số điện thoại đã được sử dụng");
                }
            }
            
            // Update fields manually to avoid overwriting with null values
            if (!string.IsNullOrEmpty(updateData.Email)) existingUser.Email = updateData.Email;
            if (!string.IsNullOrEmpty(updateData.Phone)) existingUser.Phone = updateData.Phone;
            if (!string.IsNullOrEmpty(updateData.FirstName)) existingUser.FirstName = updateData.FirstName;
            if (!string.IsNullOrEmpty(updateData.LastName)) existingUser.LastName = updateData.LastName;
            if (updateData.AvatarFileId.HasValue) existingUser.AvatarFileId = updateData.AvatarFileId;
            if (!string.IsNullOrEmpty(updateData.Gender)) existingUser.Gender = updateData.Gender;
            if (updateData.BirthDate.HasValue) existingUser.BirthDate = updateData.BirthDate;
            if (updateData.Height.HasValue) existingUser.Height = updateData.Height;
            if (updateData.Weight.HasValue) existingUser.Weight = updateData.Weight;
            if (!string.IsNullOrEmpty(updateData.SkinTone)) existingUser.SkinTone = updateData.SkinTone;
            if (!string.IsNullOrEmpty(updateData.BodyType)) existingUser.BodyType = updateData.BodyType;
            if (!string.IsNullOrEmpty(updateData.StylePreferences)) existingUser.StylePreferences = updateData.StylePreferences;
            if (!string.IsNullOrEmpty(updateData.SizePreferences)) existingUser.SizePreferences = updateData.SizePreferences;
            
            // Update boolean flags - these should be explicitly set from the request
            existingUser.IsPremium = updateData.IsPremium;
            existingUser.IsActive = updateData.IsActive;
            existingUser.ModifiedDate = DateTime.UtcNow;
            
            Repo.Update(existingUser);
            await Repo.SaveAsync();
            
            return existingUser;
        }
    }
}
