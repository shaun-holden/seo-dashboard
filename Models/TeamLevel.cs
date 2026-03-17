using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class TeamLevel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Team level name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Athlete count must be between 0 and 10,000.")]
        public int AthleteCount { get; set; }

        [Range(1, 36, ErrorMessage = "Payment plan months must be between 1 and 36.")]
        public int PaymentPlanMonths { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        // Audit timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AthleteItem> AthleteItems { get; set; } = new List<AthleteItem>();
        public ICollection<TeamLevelGroupAssignment> GroupAssignments { get; set; } = new List<TeamLevelGroupAssignment>();
        public ICollection<MeetTeamLevelAssignment> MeetAssignments { get; set; } = new List<MeetTeamLevelAssignment>();
        public ICollection<SharedFeeTeamLevelAssignment> SharedFeeAssignments { get; set; } = new List<SharedFeeTeamLevelAssignment>();
        public decimal AthleteItemsTotal => AthleteItems.Sum(i => i.Cost);
        public decimal MonthlyPayment(decimal sharedCost) => (sharedCost + AthleteItemsTotal) / PaymentPlanMonths;
    }

    public class AthleteItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Cost must be between 0 and 1,000,000.")]
        public decimal Cost { get; set; }
        public bool IsRequired { get; set; } = true;
        public int TeamLevelId { get; set; }
        public TeamLevel TeamLevel { get; set; } = null!;
    }
}
