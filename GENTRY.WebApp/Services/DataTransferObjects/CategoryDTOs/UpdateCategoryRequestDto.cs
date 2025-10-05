using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.CategoryDTOs
{
    public class UpdateCategoryRequestDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }

        public int? ParentId { get; set; }

        public int? ImageFileId { get; set; }

        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
    }
}
