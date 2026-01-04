using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Helpers;
using POS_API.Interfaces;
using POS_API.Services;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsApiController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsApiController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var pagedTags = await _tagService.GetPagedAsync(pageNumber, pageSize, search);
            var response = ApiPagedResponse<TagDto>.Success(pagedTags, "Tags retrieved successfully");

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if(tag == null)
            {
                return NotFound(new ApiResponse<TagDto>($"Tag with ID {id} not found"));
            }

            var response = new ApiResponse<TagDto>(tag, "Tag retrieved successfully");

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagCreateDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<TagDto>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            try
            {
                var newTag = await _tagService.CreateAsync(tagDto);
                var response = new ApiResponse<TagDto>(newTag, "Tag created successfully");

                return CreatedAtAction(nameof(GetById), new { id = newTag.Id }, response);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] TagUpdateDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany( v =>  v.Errors).First().ErrorMessage;
                return BadRequest(new ApiResponse<object>(error ?? "Invalid data"));
            }
            try
            {
                await _tagService.UpdateAsync(id, tagDto);
                return Ok(new ApiResponse<object>(null, "Tag updated successfully"));
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
                await _tagService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(null, "Tag deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message));
            }
        }


    }
}
