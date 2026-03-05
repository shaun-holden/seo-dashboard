namespace GymBudgetApp.Models
{
    public class MeetTeamLevelAssignment
    {
        public int Id { get; set; }
        public int MeetId { get; set; }
        public Meet Meet { get; set; } = null!;
        public int TeamLevelId { get; set; }
        public TeamLevel TeamLevel { get; set; } = null!;
    }
}
