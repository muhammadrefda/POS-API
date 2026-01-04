using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using POS_API.Helpers;

namespace POS_API.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            });
        }

        public async Task<PagedResponse<TagDto>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var (tags, totalRecords) = await _tagRepository.GetPagedAsync(pageNumber, pageSize, searchTerm);
            
            var tagDtos = tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            });

            return new PagedResponse<TagDto>(tagDtos, totalRecords, pageNumber, pageSize);
        }

        public async Task<TagDto> GetByIdAsync(long id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            return new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            };
        }

        public async Task<TagDto> CreateAsync(TagCreateDto tagCreateDto)
        {
            var tag = new Tag
            {
                TagName = tagCreateDto.TagName
            };

            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveChangesAsync();

            return new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            };
        }
        public async Task UpdateAsync(long id, TagUpdateDto tagUpdateDto)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found");
            }

            tag.TagName = tagUpdateDto.TagName;
            tag.UpdatedAt = DateTime.Now;

            _tagRepository.Update(tag);

            await _tagRepository.SaveChangesAsync();
        }
        public async Task DeleteAsync(long id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if(tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found");
            }

            //soft delete
            tag.DeletedAt = DateTime.Now;
            _tagRepository.Update(tag);
            await _tagRepository.SaveChangesAsync();
        }
    }
}
