using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;

        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepo.GetAllAsync();

            //mapping dari List<Product> ke List<ProductDto>

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CategoryName = p.Category?.CategoryName ?? "Uncategorized",
                Price = p.Price,
                Stock = p.Stock,
                Active = p.Active,
            });
        }

        public async Task<ProductDto?> GetByIdAsync(long id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                return null;

            // mapping dari Product ke ProductDTO
            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryName = product.Category?.CategoryName ?? "Uncategorized",
                Price = product.Price,
                Stock = product.Stock,
                Active = product.Active,
            };
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto productDto)
        {
            //1. mapping dari DTO ke Entity
            var productEntity = new Product
            {
                ProductName = productDto.ProductName,
                CategoryId = productDto.CategoryId,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Active = true
            };

            //2. kirim ke repository untuk disimpan
            var newEntity = await _productRepo.CreateAsync(productEntity);

            //3. ambil data yg baru disimpan untuk di mapping lagi, dan akan kita tampilin
            var resultEntity = await _productRepo.GetByIdAsync(newEntity.Id);

            //4. mapping kembali ke DTO untuk response
            return new ProductDto
            {
                Id = resultEntity.Id,
                ProductName = resultEntity.ProductName,
                CategoryName = resultEntity.Category?.CategoryName ?? "Uncategorized",
                Price = resultEntity.Price,
                Stock = resultEntity.Stock,
                Active = resultEntity.Active,
            };
        }

        public async Task UpdateAsync(long id, ProductUpdateDto productDto)
        {
            //1. ambil data dr db
            var existingProduct = await _productRepo.GetByIdAsync(id);

            //2. cek data ada apa ga, klo ga ada lempar error
            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found");


            //3. salin nilai dari DTO ke Entity yg sudah ada
            existingProduct.ProductName = productDto.ProductName;
            existingProduct.CategoryId = productDto.CategoryId;
            existingProduct.Price = productDto.Price;
            existingProduct.Stock = productDto.Stock;
            existingProduct.Active = productDto.Active;

            //4. kirim entity yg udh di update ke repository
            await _productRepo.UpdateAsync(existingProduct);
        }

        public async Task DeleteAsync(long id)
        {
            var productToDelete = await _productRepo.GetByIdAsync(id);

            if (productToDelete == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            await _productRepo.DeleteAsync(productToDelete);
        }
    }
}
