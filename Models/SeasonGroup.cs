namespace GymBudgetApp.Models
{
    public class SeasonGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public ICollection<Coach> Coaches { get; set; } = new List<Coach>();
        public ICollection<Meet> Meets { get; set; } = new List<Meet>();
        public ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();
        public ICollection<MeetGroupAssignment> MeetAssignments { get; set; } = new List<MeetGroupAssignment>();
        public ICollection<CoachGroupAssignment> CoachAssignments { get; set; } = new List<CoachGroupAssignment>();
        public ICollection<TeamLevelGroupAssignment> TeamLevelAssignments { get; set; } = new List<TeamLevelGroupAssignment>();
    }
}
