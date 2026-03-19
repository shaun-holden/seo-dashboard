using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public enum RsvpStatus { NoResponse, Going, NotGoing, Maybe }

    public class EventRsvp
    {
        public int Id { get; set; }
        public int MeetId { get; set; }
        public Meet Meet { get; set; } = null!;
        public int AthleteId { get; set; }
        public Athlete Athlete { get; set; } = null!;
        public string ParentUserId { get; set; } = string.Empty;
        public RsvpStatus Status { get; set; } = RsvpStatus.NoResponse;

        [StringLength(500)]
        public string? Note { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
