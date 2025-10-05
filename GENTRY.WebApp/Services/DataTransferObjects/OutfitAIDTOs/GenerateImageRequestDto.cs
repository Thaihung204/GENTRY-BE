using System.ComponentModel.DataAnnotations;

namespace GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs
{
    public class GenerateImageRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public List<OutfitItemDto> OutfitItems { get; set; } = new List<OutfitItemDto>();
    }
}
