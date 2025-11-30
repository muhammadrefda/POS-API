using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface IAuthService
    {
        //method ini akan mengembalikkan pesan string, ex: "Sukses" atau lempar error
        Task<string> RegisterAsync(RegisterDto request);

        //method ini akan mengembalikkan Token (string)
        Task<string> LoginAsync(LoginDto request);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(long id);
        Task<bool> ResetPasswordAsync(long id, string newPassword);
    }
}
