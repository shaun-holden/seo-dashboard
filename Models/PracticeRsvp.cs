namespace GymBudgetApp.Models
{
    public class PracticeRsvp
    {
        public int Id { get; set; }
        public int PracticeId { get; set; }
        public Practice Practice { get; set; } = null!;
        public int AthleteId { get; set; }
        public Athlete Athlete { get; set; } = null!;
        public string ParentUserId { get; set; } = string.Empty;
        public RsvpStatus Status { get; set; } = RsvpStatus.NoResponse;
        public string? Note { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
