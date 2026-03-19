using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public enum ChatRoomType { Level, AllParents, Staff, Custom }

    public class ChatRoom
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public ChatRoomType Type { get; set; }
        public int? SeasonId { get; set; }
        public int? TeamLevelId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    }

    public class ChatMessage
    {
        public int Id { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;

        [Required]
        public string SenderUserId { get; set; } = string.Empty;

        [StringLength(200)]
        public string SenderName { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ImageData { get; set; } // base64 for photos

        public bool IsPinned { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ChatRoomMember
    {
        public int Id { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime? LastReadAt { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
