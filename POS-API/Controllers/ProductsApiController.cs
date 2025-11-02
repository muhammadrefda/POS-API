using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_API.Data;
using POS_API.Helpers;
using POS_API.Models;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get api
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).ToListAsync();

                return Ok(new ApiResponse<IEnumerable<Product>>(products, "Data produk berhasil diambil"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<Product>>($"Error Server: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(long id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    return NotFound(new ApiResponse<Product>("Produk tidak ditemukan"));

                return Ok(new ApiResponse<Product>(product, "Produk ditemukan"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Product>($"Error server (ID: {id}) : {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Product>>> PostProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new ApiResponse<Product>("Data produk tidak valid"));

            if(await _context.Products.AnyAsync(p => p.ProductName == product.ProductName))
                return BadRequest(new ApiResponse<Product>($"Produk dengan nama '{product.ProductName} sudah ada.'"));

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

                var apiResponse = new ApiResponse<Product>(product, "Produk berhasil ditambahkan");

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Product>($"Error saat menyimpan: {ex.Message}"));
            }
        }

        //PUT API
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> PutProduct(long id, [FromBody] Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
                return NotFound(new ApiResponse<object>("Data tidak ditemukan"));

            var existingCategory = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);

            if (!existingCategory)
            {
                return NotFound(new ApiResponse<object>($"Category id {id} tidak ditemukan di database"));
            }

            product.Id = id;
            product.UpdatedAt = DateTime.Now;

            _context.Entry(existingProduct).CurrentValues.SetValues(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new ApiResponse<object>("Data produk tidak ditemukan (kemungkinan sudah dihapus)"));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner Exception: " + ex.InnerException.Message;
                }
                return StatusCode(500, new ApiResponse<object>($"Error saat update: {errorMessage}"));
            }

            return Ok(new ApiResponse<object>(null, "Data berhasil diupdate"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProduct(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(new ApiResponse<object>("product tidak ditemukan."));
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>(null, "Produk berhasil dihapus."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error saat menghapus: {ex.Message}"));
            }
        }
    }
}
