using Moq;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using POS_API.Services;
using Xunit;

namespace PosApi.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProductsWithTags()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    ProductName = "Product A",
                    ProductTags = new List<ProductTag>
                    {
                        new ProductTag { Tag = new Tag { TagName = "Tag1" } },
                        new ProductTag { Tag = new Tag { TagName = "Tag2" } }
                    }
                }
            };

            _productRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            var productDto = result.First();
            Assert.Equal(2, productDto.Tags.Count);
            Assert.Contains("Tag1", productDto.Tags);
            Assert.Contains("Tag2", productDto.Tags);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddTagsToProduct()
        {
            // Arrange
            var createDto = new ProductCreateDto
            {
                ProductName = "New Product",
                TagIds = new List<long> { 1, 2 }
            };

            Product capturedProduct = null;

            _productRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p)
                .ReturnsAsync((Product p) => { p.Id = 10; return p; });

            _productRepoMock.Setup(repo => repo.GetByIdAsync(10))
                .ReturnsAsync((long id) => 
                {
                    // Simulate fetching the created product with tags loaded (mocking what the repo would do)
                    capturedProduct.ProductTags = new List<ProductTag> 
                    { 
                        new ProductTag { TagId = 1, Tag = new Tag { TagName = "MockTag1" } },
                        new ProductTag { TagId = 2, Tag = new Tag { TagName = "MockTag2" } }
                    };
                    return capturedProduct;
                });

            // Act
            var result = await _productService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(capturedProduct);
            Assert.Equal(2, capturedProduct.ProductTags.Count);
            Assert.Contains(capturedProduct.ProductTags, pt => pt.TagId == 1);
            Assert.Contains(capturedProduct.ProductTags, pt => pt.TagId == 2);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceTags()
        {
            // Arrange
            var existingProduct = new Product
            {
                Id = 1,
                ProductName = "Old Name",
                ProductTags = new List<ProductTag>
                {
                    new ProductTag { TagId = 99, Tag = new Tag { TagName = "OldTag" } }
                }
            };

            var updateDto = new ProductUpdateDto
            {
                ProductName = "New Name",
                TagIds = new List<long> { 5, 6 }
            };

            _productRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);

            // Act
            await _productService.UpdateAsync(1, updateDto);

            // Assert
            Assert.Equal("New Name", existingProduct.ProductName);
            Assert.Equal(2, existingProduct.ProductTags.Count);
            Assert.Contains(existingProduct.ProductTags, pt => pt.TagId == 5);
            Assert.Contains(existingProduct.ProductTags, pt => pt.TagId == 6);
            Assert.DoesNotContain(existingProduct.ProductTags, pt => pt.TagId == 99);
            
            _productRepoMock.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }
    }
}
