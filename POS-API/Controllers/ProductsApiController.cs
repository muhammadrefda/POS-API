using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_API.DTOs;
using POS_API.Helpers;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsApiController(IProductService productService)
        {
            _productService = productService;
        }

        //get api
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var pagedProducts = await _productService.GetPagedAsync(pageNumber, pageSize, search);

            var response = ApiPagedResponse<ProductDto>.Success(pagedProducts, "Products retrieved successfully");

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                var errorResponse = new ApiResponse<ProductDto>($"Product with ID {id} not found");

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<ProductDto>(product, "Product retrieved successfully");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                //disini kita ambil error dari model state
                var errors = ModelState.Values.SelectMany( v => v.Errors).First().ErrorMessage;

                var errorResponse = new ApiResponse<ProductDto>(errors ?? "Invalid data provided");

                return BadRequest(errorResponse);
            }

            var newProduct = await _productService.CreateAsync(productDto);

            var response = new ApiResponse<ProductDto>(newProduct, "Product created successfully");

            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, response);
        }

        //PUT API
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                //disini kita ambil error dari model state
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;

                var errorResponse = new ApiResponse<object>(errors ?? "Invalid data provided");

                return BadRequest(errorResponse);
            }
            
            try
            {
                await _productService.UpdateAsync(id, productDto);

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
                var errorResponse = new ApiResponse<object>($"An error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
    }
}
