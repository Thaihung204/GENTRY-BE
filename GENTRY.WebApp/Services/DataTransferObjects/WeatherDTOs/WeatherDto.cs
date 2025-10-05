using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.WeatherDTOs
{
    public class WeatherDto
    {
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
        
        [MaxLength(100)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? Icon { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
