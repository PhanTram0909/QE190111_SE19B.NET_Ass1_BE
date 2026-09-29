using System.ComponentModel.DataAnnotations;

namespace TaskTrack.Service.DTOs
{
    // ─── Read ────────────────────────────────────────────────────────────────────
    public class TagReadDto
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string? Color { get; set; }
    }

    // ─── Create ──────────────────────────────────────────────────────────────────
    public class TagCreateDto
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [MaxLength(100)]
        public string TagName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Color { get; set; }
    }

    // ─── Update ──────────────────────────────────────────────────────────────────
    public class TagUpdateDto
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [MaxLength(100)]
        public string TagName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Color { get; set; }
    }
}
