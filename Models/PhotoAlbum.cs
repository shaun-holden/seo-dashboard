using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class PhotoAlbum
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        // null = all levels can see
        public int? TeamLevelId { get; set; }

        // Optional link to a meet
        public int? MeetId { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }

    public class Photo
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }
        public PhotoAlbum Album { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = string.Empty;

        // Store as base64 data URI in the database for simplicity (small photos)
        // For production, use cloud storage
        [Required]
        public string ImageData { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Caption { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
