using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.ItemDTOs
{
    public class AddItemRequest
    {
        [Required(ErrorMessage = "Tên trang phục là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên trang phục không được vượt quá 255 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public int CategoryId { get; set; }

        [MaxLength(100, ErrorMessage = "Thương hiệu không được vượt quá 100 ký tự")]
        public string? Brand { get; set; }

        public int? ColorId { get; set; }
        [MaxLength(500, ErrorMessage = "Tags không được vượt quá 500 ký tự")]
        public string? Tags { get; set; }

        // Cho việc upload ảnh
        public IFormFile? ImageFile { get; set; }
    }
}
