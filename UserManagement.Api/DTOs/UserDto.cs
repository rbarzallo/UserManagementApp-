namespace UserManagement.Api.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string TwoFactorSecret { get; set; }
    }
}
