namespace Domain.DTOs
{
    public class UserDTO
    {
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Id { get; set; }
        public string? Image { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
