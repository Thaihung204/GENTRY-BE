namespace GENTRY.WebApp.Services.DataTransferObjects.FeedbackDTOs
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; } = null!;
        public Guid? UserId { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

