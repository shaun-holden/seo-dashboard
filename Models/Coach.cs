using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class Coach
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Coach name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public int? SeasonGroupId { get; set; }
        public SeasonGroup? SeasonGroup { get; set; }

        // Audit timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PerDiemEntry> PerDiemEntries { get; set; } = new List<PerDiemEntry>();
        public ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();
        public ICollection<CoachMeetAssignment> MeetAssignments { get; set; } = new List<CoachMeetAssignment>();
        public ICollection<CoachGroupAssignment> GroupAssignments { get; set; } = new List<CoachGroupAssignment>();
    }
}
