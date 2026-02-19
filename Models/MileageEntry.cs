namespace GymBudgetApp.Models
{
    public class MileageEntry
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int MeetId { get; set; }
        public decimal Miles { get; set; }
        public decimal RatePerMile { get; set; }
        public decimal Total => Miles * RatePerMile;
        public bool IsActual { get; set; }
        public Coach Coach { get; set; } = null!;
        public Meet Meet { get; set; } = null!;
    }
}
