using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Models
{
    [Table("Feedbacks")]
    public partial class Feedback : Entity<Guid>
    {
        /// <summary>
        /// Tên của người feedback
        /// </summary>
        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Email của người feedback (optional)
        /// </summary>
        [MaxLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Đánh giá từ 1 đến 5 sao
        /// </summary>
        [Required]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }

        /// <summary>
        /// Nội dung phản hồi
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; } = null!;

        /// <summary>
        /// User ID nếu người feedback đã đăng nhập (optional)
        /// </summary>
        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// Trạng thái hiển thị (để admin có thể ẩn feedback không phù hợp)
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Navigation property đến User
        /// </summary>
        public virtual User? User { get; set; }
    }
}
