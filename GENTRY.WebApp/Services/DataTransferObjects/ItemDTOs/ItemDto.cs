using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GENTRY.WebApp.Services.DataTransferObjects.ItemDTOs
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        
        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? Brand { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int? FileId { get; set; }
        public string? FileUrl { get; set; }

        [MaxLength(500)]
        public string? SourceUrl { get; set; }

        public string? Description { get; set; }

        public int? ColorId { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHex { get; set; }

        [MaxLength(20)]
        public string? Size { get; set; }

        [MaxLength(500)]
        public string? Tags { get; set; }

        public decimal? Price { get; set; }
        public DateTime? PurchaseDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
