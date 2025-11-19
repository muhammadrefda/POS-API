using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Helpers;
using POS_API.Interfaces;

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
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            var response = new ApiResponse<IEnumerable<TagDto>>(tags, "Tags retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
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
                return BadRequest(new ApiResponse<TagDto>(errors ?? "Invalid data"));
            }

            var newTag = await _tagService.CreateAsync(tagDto);
            var response = new ApiResponse<TagDto>(newTag, "Tag created successfully");
            return CreatedAtAction(nameof(GetById), new { id = newTag.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] TagUpdateDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                return BadRequest(new ApiResponse<object>(errors ?? "Invalid data"));
            }

            try
            {
                await _tagService.UpdateAsync(id, tagDto);
                return Ok(new ApiResponse<object>(null, "Tag updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message));
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