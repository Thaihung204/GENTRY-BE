using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.ColorDTOs
{
    public class ColorDto
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(10)]
        public string? HexCode { get; set; }

        [MaxLength(20)]
        public string? RgbValues { get; set; }

        [MaxLength(50)]
        public string? ColorFamily { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
