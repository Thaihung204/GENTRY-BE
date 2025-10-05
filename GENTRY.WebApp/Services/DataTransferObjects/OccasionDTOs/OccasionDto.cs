using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.OccasionDTOs
{
    public class OccasionDto
    {
        public int Id { get; set; }
        
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [MaxLength(255)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? Category { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
