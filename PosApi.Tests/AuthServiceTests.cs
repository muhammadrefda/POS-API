using Microsoft.Extensions.Configuration;
using Moq;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using POS_API.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosApi.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IAuthRepository>();
            _mockConfig = new Mock<IConfiguration>();

            var dummyEncodeKey = "VGhpcyBpcyBhIHZlcnkgbG9uZyBzZWNyZXQga2V5IHRvIHNhdGlzZnkgdGhlIEhNQUNTSEE1MTIgcmVxdWlyZW1lbnRzIGZvciB1bml0IHRlc3RpbmcuIFBsZWFzZSBkb24ndCB1c2UgdGhpcyBpbiBwcm9kdWN0aW9uIQ";

            var mockSection = new Mock<IConfigurationSection>();

            mockSection.Setup(s => s.Value).Returns(dummyEncodeKey);

            _mockConfig.Setup(c => c.GetSection("JwtSettings:SecretKey")).Returns(mockSection.Object);

            _mockConfig.Setup(c => c["JwtSettings:Issuer"]).Returns("PosApi");
            _mockConfig.Setup(c => c["JwtSettings:Audience"]).Returns("PosClient");

            //inject ke Service
            _authService = new AuthService(_mockConfig.Object, _mockRepo.Object);

        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var loginDto = new LoginDto
            {
                Username = "abz21",
                Password = "Admin123*"
            };

            //seakan akan ada user di db
            var dummyUser = new User
            {
                Id = 1,
                Username = "abz21",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"),
                Role = "Admin"
            };

            _mockRepo.Setup(repo => repo.GetUserByUsernameAsync("abz21")).ReturnsAsync(dummyUser);

            //eksekusinya

            var result = await _authService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
        {
            var loginDto = new LoginDto
            {
                Username = "lana21",
                Password = "Abc1234"
            };

            _mockRepo.Setup(repo => repo.GetUserByUsernameAsync("lana21")).ReturnsAsync((User)null);

            //act & assert

            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(loginDto));


            Assert.Equal("Username or Password is wrong", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordWrong()
        {
            var loginDto = new LoginDto
            {
                Username = "lana21",
                Password = "PASSWORD_SALAH"
            };

            var dummyUser = new User
            {
                Id = 1,
                Username = "lana21",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"),
                Role = "Admin"
            };

            _mockRepo.Setup(repo => repo.GetUserByUsernameAsync("lana21")).ReturnsAsync(dummyUser);

            //act & assert

            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(loginDto));

            Assert.Equal("Username or Password is wrong", exception.Message);
        }
    }
}
