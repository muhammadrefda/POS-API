namespace POS_API.DTOs
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
    }
}
