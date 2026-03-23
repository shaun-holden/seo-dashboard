using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public enum ProjectType { Competition, Custom }

    public class Season
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Season name is required.")]
        [StringLength(100, ErrorMessage = "Season name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Athlete count must be between 0 and 10,000.")]
        public int AthleteCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Project type
        public ProjectType ProjectType { get; set; } = ProjectType.Competition;

        // Custom labels (used when ProjectType == Custom)
        [StringLength(50)]
        public string MeetLabel { get; set; } = "Meet";
        [StringLength(50)]
        public string CoachLabel { get; set; } = "Coach";
        [StringLength(50)]
        public string TeamLevelLabel { get; set; } = "Team Level";
        [StringLength(50)]
        public string AthleteItemLabel { get; set; } = "Athlete Item";

        // Feature toggles (used when ProjectType == Custom)
        public bool EnableTeamLevels { get; set; } = true;
        public bool EnablePerDiem { get; set; } = true;
        public bool EnableMileage { get; set; } = true;

        // Payment settings
        public int PaymentMonths { get; set; } = 8;
        public int PaymentStartMonth { get; set; } = 8; // 1-12 (default August)

        // Audit timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Meet> Meets { get; set; } = new List<Meet>();
        public ICollection<Coach> Coaches { get; set; } = new List<Coach>();
        public ICollection<TeamLevel> TeamLevels { get; set; } = new List<TeamLevel>();
        public ICollection<SeasonGroup> SeasonGroups { get; set; } = new List<SeasonGroup>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<SharedFee> SharedFees { get; set; } = new List<SharedFee>();
        public ICollection<SeasonNote> SeasonNotes { get; set; } = new List<SeasonNote>();
    }
}
