namespace BlackDragonAPI.Models
{
    public class DeleteUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
