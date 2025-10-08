using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.StyleDTOs
{
    public class StyleDto
    {
        public int StyleId { get; set; }
        
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }

        public int? ImageFileId { get; set; }
        
        public string? Tags { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
