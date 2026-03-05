namespace GymBudgetApp.Models
{
    public class MeetGroupAssignment
    {
        public int Id { get; set; }
        public int MeetId { get; set; }
        public Meet Meet { get; set; } = null!;
        public int SeasonGroupId { get; set; }
        public SeasonGroup SeasonGroup { get; set; } = null!;
    }
}
