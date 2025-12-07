using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface IAuthRepository
    {
        // Cek apakah user ada (untuk validasi register)
        Task<bool> UserExistsAsync(string username);
        // Simpan user baru
        Task CreateUserAsync(User user);
        // Ambil user berdasarkan username (untuk login)
        Task<User?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
