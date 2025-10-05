using GENTRY.WebApp.Services.DataTransferObjects.OccasionDTOs;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class OccasionService : IOccasionService
    {
        private readonly List<OccasionDto> _occasionData;

        public OccasionService()
        {
            // Initialize predefined occasions
            _occasionData = new List<OccasionDto>
            {
                // Work/Business
                new OccasionDto { Id = 1, Name = "Work Meeting", Description = "Họp công việc", Category = "Work", IsActive = true },
                new OccasionDto { Id = 2, Name = "Business Presentation", Description = "Thuyết trình công việc", Category = "Work", IsActive = true },
                new OccasionDto { Id = 3, Name = "Job Interview", Description = "Phỏng vấn xin việc", Category = "Work", IsActive = true },
                new OccasionDto { Id = 4, Name = "Office Party", Description = "Tiệc công ty", Category = "Work", IsActive = true },
                
                // Social/Party
                new OccasionDto { Id = 5, Name = "Date Night", Description = "Hẹn hò", Category = "Social", IsActive = true },
                new OccasionDto { Id = 6, Name = "Birthday Party", Description = "Tiệc sinh nhật", Category = "Social", IsActive = true },
                new OccasionDto { Id = 7, Name = "Wedding", Description = "Đám cưới", Category = "Social", IsActive = true },
                new OccasionDto { Id = 8, Name = "Cocktail Party", Description = "Tiệc cocktail", Category = "Social", IsActive = true },
                new OccasionDto { Id = 9, Name = "Dinner Party", Description = "Tiệc tối", Category = "Social", IsActive = true },
                
                // Casual
                new OccasionDto { Id = 10, Name = "Casual Outing", Description = "Đi chơi thường ngày", Category = "Casual", IsActive = true },
                new OccasionDto { Id = 11, Name = "Shopping", Description = "Đi mua sắm", Category = "Casual", IsActive = true },
                new OccasionDto { Id = 12, Name = "Coffee Date", Description = "Hẹn cafe", Category = "Casual", IsActive = true },
                new OccasionDto { Id = 13, Name = "Weekend Brunch", Description = "Brunch cuối tuần", Category = "Casual", IsActive = true },
                
                // Sport/Active
                new OccasionDto { Id = 14, Name = "Gym Workout", Description = "Tập gym", Category = "Sport", IsActive = true },
                new OccasionDto { Id = 15, Name = "Yoga Class", Description = "Lớp yoga", Category = "Sport", IsActive = true },
                new OccasionDto { Id = 16, Name = "Running", Description = "Chạy bộ", Category = "Sport", IsActive = true },
                new OccasionDto { Id = 17, Name = "Outdoor Activities", Description = "Hoạt động ngoài trời", Category = "Sport", IsActive = true },
                
                // Formal
                new OccasionDto { Id = 18, Name = "Gala", Description = "Tiệc gala", Category = "Formal", IsActive = true },
                new OccasionDto { Id = 19, Name = "Awards Ceremony", Description = "Lễ trao giải", Category = "Formal", IsActive = true },
                new OccasionDto { Id = 20, Name = "Formal Dinner", Description = "Bữa tối trang trọng", Category = "Formal", IsActive = true }
            };
        }

        public async Task<List<OccasionDto>> GetAllAsync()
        {
            await Task.Delay(1); // Simulate async operation
            return _occasionData.Where(o => o.IsActive).ToList();
        }

        public async Task<OccasionDto?> GetByIdAsync(int id)
        {
            await Task.Delay(1); // Simulate async operation
            return _occasionData.FirstOrDefault(o => o.Id == id && o.IsActive);
        }
    }
}
