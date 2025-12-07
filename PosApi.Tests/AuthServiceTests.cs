using Microsoft.Extensions.Configuration;
using Moq;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using POS_API.Services;
using Xunit;

public class AuthServiceTests
{
    // 1. Siapkan variable untuk Mocking (Si Penipu)
    private readonly Mock<IAuthRepository> _mockRepo;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly AuthService _authService; // Ini yang mau kita test (The Engine)

    public AuthServiceTests()
    {
        // Inisialisasi Mock
        _mockRepo = new Mock<IAuthRepository>();
        _mockConfig = new Mock<IConfiguration>();

        // --- SETUP CONFIG DISINI (Supaya dipakai semua Test) ---

        // Key baru yang sangat panjang (Valid untuk SHA512)
        var dummyEncodedKey = "VGhpcyBpcyBhIHZlcnkgbG9uZyBzZWNyZXQga2V5IHRvIHNhdGlzZnkgdGhlIEhNQUNTSEE1MTIgcmVxdWlyZW1lbnRzIGZvciB1bml0IHRlc3RpbmcuIFBsZWFzZSBkb24ndCB1c2UgdGhpcyBpbiBwcm9kdWN0aW9uIQ==";

        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(s => s.Value).Returns(dummyEncodedKey);

        _mockConfig.Setup(c => c.GetSection("JwtSettings:SecretKey"))
                   .Returns(mockSection.Object);

        _mockConfig.Setup(c => c["JwtSettings:Issuer"]).Returns("PosApi");
        _mockConfig.Setup(c => c["JwtSettings:Audience"]).Returns("PosClient");

        // Inject ke Service
        _authService = new AuthService(_mockConfig.Object, _mockRepo.Object);
    }

    [Fact]
    /*
     **[Fact]** ≠ Array, dia keyword dari Unit Test .net, 
     *untuk nandain kalau method ini ketika dijalankan Unit testing, bisa kita jalankan
     */
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // --- ARRANGE (Persiapan Skenario) ---
        var loginDto = new LoginDto { Username = "ali_admin", Password = "password123" };

        // Pura-pura ada user di database
        var dummyUser = new User
        {
            Id = 1,
            Username = "ali_admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), // Hash password simulasi
            Role = "Admin"
        };

        // Ajari Mock Repo: "Kalau ada yang tanya user 'ali_admin', kasih dummyUser ini ya."
        _mockRepo.Setup(repo => repo.GetUserByUsernameAsync("ali_admin"))
                 .ReturnsAsync(dummyUser);

        // --- ACT (Eksekusi) ---
        var result = await _authService.LoginAsync(loginDto);


        /*
         Penjelasan ASSERT:
        Assert dibuat static method biar lebih simpel oleh XUnit

        Assert merupakan sebuah kelas dan di dalam nya ada static method, 
        makanya ga perlu ada instantiate object baru (var assert = new Assert();)
         */

        // --- ASSERT (Verifikasi) ---
        Assert.NotNull(result); // Pastikan tidak null
        Assert.IsType<string>(result); // Pastikan return-nya string (Token)
        Assert.True(result.Length > 0); // Pastikan string tidak kosong
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
    {
        // --- ARRANGE ---
        var loginDto = new LoginDto { Username = "hacker", Password = "123" };

        // Ajari Mock Repo: "Kalau tanya user 'hacker', bilang gak ada (null)."
        _mockRepo.Setup(repo => repo.GetUserByUsernameAsync("hacker"))
                 .ReturnsAsync((User)null);

        // --- ACT & ASSERT ---
        // Kita ekspektasi Service akan melempar Exception
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _authService.LoginAsync(loginDto));

        Assert.Equal("Username or Password is wrong", exception.Message); // Sesuaikan dengan pesan error di Service Anda
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenPasswordWrong()
    {
        // --- ARRANGE ---
        var loginDto = new LoginDto { Username = "ali_admin", Password = "WRONG_PASSWORD" };

        var dummyUser = new User
        {
            Id = 1,
            Username = "ali_admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "Admin"
        };

        _mockRepo.Setup(repo => repo.GetUserByUsernameAsync("ali_admin"))
                 .ReturnsAsync(dummyUser);

        // --- ACT & ASSERT ---
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _authService.LoginAsync(loginDto));

        Assert.Equal("Username or Password is wrong", exception.Message); // Sesuaikan pesan error
    }
}