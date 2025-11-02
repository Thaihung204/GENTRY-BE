using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.FeedbackDTOs
{
    public class FeedbackRequest
    {
        [Required(ErrorMessage = "Tên là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Đánh giá là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Nội dung phản hồi là bắt buộc")]
        [MaxLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự")]
        public string Content { get; set; } = null!;
    }
}

