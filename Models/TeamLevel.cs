namespace GymBudgetApp.Models
{
    public class TeamLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AthleteCount { get; set; }
        public int PaymentPlanMonths { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
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
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public bool IsRequired { get; set; } = true;
        public int TeamLevelId { get; set; }
        public TeamLevel TeamLevel { get; set; } = null!;
    }
}