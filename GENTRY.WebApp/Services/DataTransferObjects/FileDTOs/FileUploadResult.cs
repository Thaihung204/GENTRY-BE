namespace GENTRY.WebApp.Services.DataTransferObjects.FileDTOs
{
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string? Url { get; set; }
        public string? PublicId { get; set; }
        public string? ErrorMessage { get; set; }
        public long? FileSize { get; set; }
        public string? Format { get; set; }
    }
} 