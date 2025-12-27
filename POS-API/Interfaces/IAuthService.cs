using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface IAuthService
    {
        // Method Register mengembalikan pesan string (misal: "Sukses") atau throw error
        Task<string> RegisterAsync(RegisterDto request);

        // Method Login mengembalikan Token (string)
        Task<string> LoginAsync(LoginDto request);
    }
}