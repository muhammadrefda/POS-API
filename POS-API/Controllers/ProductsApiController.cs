using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_API.Data;
using POS_API.Models;
using POS_API.Helpers; // <-- 1. JANGAN LUPA using helper baru kita

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

        // GET: api/ProductsApi
        // Perubahan 1: Return type
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts()
        {
            try
            {
                var products = await _context.Products
                                             .Include(p => p.Category)
                                             .ToListAsync();

                // Perubahan 2: Bungkus dengan ApiResponse sukses
                return Ok(new ApiResponse<IEnumerable<Product>>(products, "Data produk berhasil diambil"));
            }
            catch (Exception ex)
            {
                // Perubahan 3: Bungkus dengan ApiResponse gagal
                return StatusCode(500, new ApiResponse<IEnumerable<Product>>($"Error server: {ex.Message}"));
            }
        }

        // GET: api/ProductsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(long id)
        {
            try
            {
                var product = await _context.Products
                                            .Include(p => p.Category)
                                            .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    // Perubahan 4: Respon NotFound yang standar
                    return NotFound(new ApiResponse<Product>("Produk tidak ditemukan."));
                }

                return Ok(new ApiResponse<Product>(product, "Produk ditemukan."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Product>($"Error server (ID: {id}): {ex.Message}"));
            }
        }

        // POST: api/ProductsApi
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Product>>> PostProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new ApiResponse<Product>("Data produk tidak valid."));
            }

            if (await _context.Products.AnyAsync(p => p.ProductName == product.ProductName))
            {
                // Perubahan 5: Respon BadRequest yang standar
                return BadRequest(new ApiResponse<Product>($"Produk dengan nama '{product.ProductName}' sudah ada."));
            }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

                // Perubahan 6: Bungkus data di CreatedAtAction
                var apiResponse = new ApiResponse<Product>(product, "Produk berhasil ditambahkan.");
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Product>($"Error saat menyimpan: {ex.Message}"));
            }
        }

        // PUT: api/ProductsApi/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> PutProduct(long id, [FromBody] Product product) // Return ApiResponse<object>
        {
            if (id != product.Id)
            {
                return BadRequest(new ApiResponse<object>("ID produk tidak cocok."));
            }

            if (await _context.Products.AnyAsync(p => p.ProductName == product.ProductName && p.Id != id))
            {
                return BadRequest(new ApiResponse<object>("Nama produk tersebut sudah digunakan oleh produk lain."));
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new ApiResponse<object>("Data produk tidak ditemukan (kemungkinan sudah dihapus)."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error saat update: {ex.Message}"));
            }

            // Perubahan 7: Ganti NoContent() dengan Ok() agar bisa kirim pesan
            return Ok(new ApiResponse<object>(null, "Produk berhasil diperbarui."));
        }

        // DELETE: api/ProductsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProduct(long id) // Return ApiResponse<object>
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(new ApiResponse<object>("Produk tidak ditemukan."));
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                // Perubahan 8: Ganti NoContent() dengan Ok()
                return Ok(new ApiResponse<object>(null, "Produk berhasil dihapus."));
            }
            catch (Exception ex)
            {
                // Ini akan menangkap error foreign key
                return StatusCode(500, new ApiResponse<object>($"Error saat menghapus: {ex.Message}. (Mungkin produk sudah ada di transaksi?)"));
            }
        }
    }
}