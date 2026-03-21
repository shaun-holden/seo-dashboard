using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class ApparelItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsRequired { get; set; }

        // Optional: specific to certain levels (comma-separated), null = all levels
        [StringLength(500)]
        public string? AppliesTo { get; set; }

        public DateTime? SelectionDeadline { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
