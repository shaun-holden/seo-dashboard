namespace GymBudgetApp.Models
{
    public class TeamLevelGroupAssignment
    {
        public int Id { get; set; }
        public int TeamLevelId { get; set; }
        public TeamLevel TeamLevel { get; set; } = null!;
        public int SeasonGroupId { get; set; }
        public SeasonGroup SeasonGroup { get; set; } = null!;
    }
}
