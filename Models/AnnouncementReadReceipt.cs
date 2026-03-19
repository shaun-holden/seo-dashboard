namespace GymBudgetApp.Models
{
    public class AnnouncementReadReceipt
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }
}
