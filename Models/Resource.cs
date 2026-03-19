using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public enum ResourceCategory { Handbook, Policies, Forms, Other }

    public class Resource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public ResourceCategory Category { get; set; }

        [StringLength(1000)]
        public string? Url { get; set; } // External link

        public string? FileData { get; set; } // Base64 for uploaded files

        [StringLength(100)]
        public string? FileName { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        public bool IsRequired { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
