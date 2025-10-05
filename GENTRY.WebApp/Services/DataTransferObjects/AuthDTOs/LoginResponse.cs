namespace GENTRY.WebApp.Services.DataTransferObjects.AuthDTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
