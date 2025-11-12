using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Helpers; // <-- Jangan lupa tambahkan ini

namespace POS_API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController : ControllerBase
    
    {
        private readonly IProductService _productService;

        public ProductsApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            // Bungkus data dengan ApiResponse
            var response = new ApiResponse<IEnumerable<ProductDto>>(products, "Products retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                // Kirim response error 404 menggunakan ApiResponse
                var errorResponse = new ApiResponse<ProductDto>($"Product with ID {id} not found.");
                return NotFound(errorResponse);
            }

            // Bungkus data dengan ApiResponse
            var response = new ApiResponse<ProductDto>(product, "Product retrieved successfully");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                // Ambil error dari ModelState dan kirim sebagai ApiResponse
                // Ini adalah cara sederhana, bisa dibuat lebih kompleks untuk list semua error
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<ProductDto>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            var newProduct = await _productService.CreateAsync(productDto);

            // Bungkus data dengan ApiResponse
            var response = new ApiResponse<ProductDto>(newProduct, "Product created successfully");

            // Kembalikan 201 Created
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<object>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            try
            {
                await _productService.UpdateAsync(id, productDto);

                // Ubah 204 NoContent menjadi 200 OK dengan body ApiResponse
                // Menggunakan 'object' sebagai T karena kita tidak mengembalikan data spesifik
                var response = new ApiResponse<object>(null, "Product updated successfully");
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponse = new ApiResponse<object>(ex.Message);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                // Penanganan error umum
                var errorResponse = new ApiResponse<object>($"An error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _productService.DeleteAsync(id);

                // Ubah 204 NoContent menjadi 200 OK dengan body ApiResponse
                var response = new ApiResponse<object>(null, "Product deleted successfully");
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponse = new ApiResponse<object>(ex.Message);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                // Penanganan error umum
                var errorResponse = new ApiResponse<object>($"An error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
    }
}