namespace GymBudgetApp.Models
{
    public class SharedFeeTeamLevelAssignment
    {
        public int Id { get; set; }
        public int SharedFeeId { get; set; }
        public SharedFee SharedFee { get; set; } = null!;
        public int TeamLevelId { get; set; }
        public TeamLevel TeamLevel { get; set; } = null!;
    }
}
