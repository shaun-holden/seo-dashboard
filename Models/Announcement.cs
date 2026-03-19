using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        // null = all levels in season, otherwise specific level
        public int? TeamLevelId { get; set; }

        public string AuthorUserId { get; set; } = string.Empty;

        [StringLength(200)]
        public string AuthorEmail { get; set; } = string.Empty;

        public bool IsPinned { get; set; }
        public bool SendEmail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
