using TaskTrack.Repo.Models;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _uow;

        public TagService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<TagReadDto>> GetAllAsync()
        {
            var tags = await _uow.Tags.GetAllTagsAsync();
            return tags.Select(MapToReadDto);
        }

        public async Task<TagReadDto> CreateAsync(TagCreateDto dto)
        {
            var entity = new Tag
            {
                TagName = dto.TagName,
                Color = dto.Color
            };

            await _uow.Tags.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<TagReadDto?> UpdateAsync(int id, TagUpdateDto dto)
        {
            var entity = await _uow.Tags.GetByIdAsync(id);
            if (entity == null) return null;

            entity.TagName = dto.TagName;
            entity.Color = dto.Color;

            _uow.Tags.Update(entity);
            await _uow.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.Tags.GetByIdAsync(id);
            if (entity == null) return false;

            var isUsed = await _uow.Tags.IsUsedByTaskAsync(id);
            if (isUsed)
                throw new InvalidOperationException(
                    "Cannot delete tag because it is currently assigned to one or more tasks. Remove the tag from all tasks first.");

            _uow.Tags.Delete(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        // ─── Mapping helper ───────────────────────────────────────────────────────
        private static TagReadDto MapToReadDto(Tag t) => new()
        {
            TagID = t.TagID,
            TagName = t.TagName,
            Color = t.Color
        };
    }
}
