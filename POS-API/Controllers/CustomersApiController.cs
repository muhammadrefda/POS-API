using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Helpers; // <-- Jangan lupa

namespace POS_API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersApiController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersApiController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            var response = new ApiResponse<IEnumerable<CustomerDto>>(customers, "Customers retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                var errorResponse = new ApiResponse<CustomerDto>($"Customer with ID {id} not found.");
                return NotFound(errorResponse);
            }
            var response = new ApiResponse<CustomerDto>(customer, "Customer retrieved successfully");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<CustomerDto>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            try
            {
                var newCustomer = await _customerService.CreateAsync(customerDto);
                var response = new ApiResponse<CustomerDto>(newCustomer, "Customer created successfully");
                return CreatedAtAction(nameof(GetById), new { id = newCustomer.Id }, response);
            }
            catch (Exception ex) // Menangkap error validasi email dari Service
            {
                var errorResponse = new ApiResponse<object>($"An error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] CustomerUpdateDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<object>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            try
            {
                await _customerService.UpdateAsync(id, customerDto);
                var response = new ApiResponse<object>(null, "Customer updated successfully");
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
                await _customerService.DeleteAsync(id);
                var response = new ApiResponse<object>(null, "Customer deleted successfully");
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