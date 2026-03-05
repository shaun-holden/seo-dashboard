namespace GymBudgetApp.Models
{
    public enum TravelType { Drive, Fly }
    public class Meet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TravelType TravelType { get; set; }
        public decimal BudgetAmount { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public int? SeasonGroupId { get; set; }
        public SeasonGroup? SeasonGroup { get; set; }
        public ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();
        public ICollection<PerDiemEntry> PerDiemEntries { get; set; } = new List<PerDiemEntry>();
        public ICollection<CoachMeetAssignment> CoachAssignments { get; set; } = new List<CoachMeetAssignment>();
        public ICollection<MeetGroupAssignment> GroupAssignments { get; set; } = new List<MeetGroupAssignment>();
        public ICollection<MeetTeamLevelAssignment> TeamLevelAssignments { get; set; } = new List<MeetTeamLevelAssignment>();
    }
}