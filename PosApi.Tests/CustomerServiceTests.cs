using Moq;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using POS_API.Services;
using Xunit;

namespace PosApi.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepoMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_customerRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FullName = "John Doe", PhoneNumber = "123", Email = "john@example.com", CreatedAt = DateTime.Now },
                new Customer { Id = 2, FullName = "Jane Doe", PhoneNumber = "456", Email = "jane@example.com", CreatedAt = DateTime.Now }
            };

            _customerRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.FullName == "John Doe");
            Assert.Contains(result, c => c.FullName == "Jane Doe");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenExists()
        {
            // Arrange
            var customer = new Customer { Id = 1, FullName = "John Doe", PhoneNumber = "123", Email = "john@example.com", CreatedAt = DateTime.Now };
            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.FullName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCustomer_WhenEmailIsUnique()
        {
            // Arrange
            var createDto = new CustomerCreateDto { FullName = "New User", PhoneNumber = "789", Email = "new@example.com" };
            var createdCustomer = new Customer { Id = 10, FullName = "New User", PhoneNumber = "789", Email = "new@example.com", CreatedAt = DateTime.Now, JoinDate = DateTime.Now };

            _customerRepoMock.Setup(repo => repo.GetByEmailAsync(createDto.Email)).ReturnsAsync((Customer)null);
            _customerRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(createdCustomer);

            // Act
            var result = await _customerService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("New User", result.FullName);
            _customerRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenEmailExists()
        {
            // Arrange
            var createDto = new CustomerCreateDto { FullName = "Existing User", PhoneNumber = "789", Email = "existing@example.com" };
            var existingCustomer = new Customer { Id = 5, FullName = "Old User", Email = "existing@example.com" };

            _customerRepoMock.Setup(repo => repo.GetByEmailAsync(createDto.Email)).ReturnsAsync(existingCustomer);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _customerService.CreateAsync(createDto));
            Assert.Contains("already exists", exception.Message);
            _customerRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer_WhenExists()
        {
            // Arrange
            var updateDto = new CustomerUpdateDto { FullName = "Updated Name", PhoneNumber = "999", Email = "updated@example.com", Active = true };
            var existingCustomer = new Customer { Id = 1, FullName = "Original Name", PhoneNumber = "000", Email = "original@example.com" };

            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCustomer);

            // Act
            await _customerService.UpdateAsync(1, updateDto);

            // Assert
            Assert.Equal("Updated Name", existingCustomer.FullName);
            Assert.Equal("999", existingCustomer.PhoneNumber);
            Assert.Equal("updated@example.com", existingCustomer.Email);
            Assert.Null(existingCustomer.DeletedAt);
            _customerRepoMock.Verify(repo => repo.UpdateAsync(existingCustomer), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSoftDelete_WhenActiveIsFalse()
        {
            // Arrange
            var updateDto = new CustomerUpdateDto { FullName = "Name", Active = false };
            var existingCustomer = new Customer { Id = 1, FullName = "Name", DeletedAt = null };

            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCustomer);

            // Act
            await _customerService.UpdateAsync(1, updateDto);

            // Assert
            Assert.NotNull(existingCustomer.DeletedAt);
            _customerRepoMock.Verify(repo => repo.UpdateAsync(existingCustomer), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFound_WhenNotExists()
        {
            // Arrange
            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.UpdateAsync(1, new CustomerUpdateDto()));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete_WhenExists()
        {
            // Arrange
            var customer = new Customer { Id = 1 };
            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            await _customerService.DeleteAsync(1);

            // Assert
            _customerRepoMock.Verify(repo => repo.DeleteAsync(customer), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFound_WhenNotExists()
        {
            // Arrange
            _customerRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.DeleteAsync(1));
        }
    }
}
