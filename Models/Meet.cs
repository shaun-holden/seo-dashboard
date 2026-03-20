using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public enum TravelType { Drive, Fly }
    public class Meet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Meet name is required.")]
        [StringLength(200, ErrorMessage = "Meet name cannot exceed 200 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }
        public TravelType TravelType { get; set; }

        [Range(0, 1000000, ErrorMessage = "Budget amount must be between 0 and 1,000,000.")]
        public decimal BudgetAmount { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public int? SeasonGroupId { get; set; }
        public SeasonGroup? SeasonGroup { get; set; }

        // Venue details
        [StringLength(500)]
        public string? Address { get; set; }

        [Range(0, 100000)]
        public decimal? EntryCost { get; set; }

        [Range(0, 100000)]
        public decimal? ParkingCost { get; set; }

        [StringLength(2000)]
        public string? CompetitionSchedule { get; set; }

        [StringLength(500)]
        public string? HotelInfo { get; set; }

        [StringLength(2000)]
        public string? ParentNotes { get; set; }

        // Audit timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();
        public ICollection<PerDiemEntry> PerDiemEntries { get; set; } = new List<PerDiemEntry>();
        public ICollection<CoachMeetAssignment> CoachAssignments { get; set; } = new List<CoachMeetAssignment>();
        public ICollection<MeetGroupAssignment> GroupAssignments { get; set; } = new List<MeetGroupAssignment>();
        public ICollection<MeetTeamLevelAssignment> TeamLevelAssignments { get; set; } = new List<MeetTeamLevelAssignment>();
    }
}
