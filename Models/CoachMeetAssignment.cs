namespace GymBudgetApp.Models
{
    public class CoachMeetAssignment
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = null!;
        public int MeetId { get; set; }
        public Meet Meet { get; set; } = null!;
    }
}
