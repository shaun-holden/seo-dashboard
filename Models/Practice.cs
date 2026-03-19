using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Practice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [StringLength(10)]
        public string StartTime { get; set; } = string.Empty; // e.g. "4:00 PM"

        [StringLength(10)]
        public string EndTime { get; set; } = string.Empty; // e.g. "7:00 PM"

        [StringLength(500)]
        public string? Location { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        // null = all levels
        public int? TeamLevelId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
