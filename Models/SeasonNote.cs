using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class SeasonNote
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        public bool IsPinned { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(200)]
        public string? Author { get; set; }
    }
}
