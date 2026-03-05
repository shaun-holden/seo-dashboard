namespace GymBudgetApp.Models
{
    public class CoachGroupAssignment
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = null!;
        public int SeasonGroupId { get; set; }
        public SeasonGroup SeasonGroup { get; set; } = null!;
    }
}
