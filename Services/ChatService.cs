using GymBudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymBudgetApp.Services
{
    public class ChatService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public ChatService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task EnsureRoomsExist(int seasonId, List<TeamLevel> levels)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            // Staff Only room (one global)
            var staffRoom = await db.ChatRooms
                .FirstOrDefaultAsync(r => r.Type == ChatRoomType.Staff);
            if (staffRoom == null)
            {
                db.ChatRooms.Add(new ChatRoom
                {
                    Name = "Staff Only",
                    Type = ChatRoomType.Staff
                });
            }

            // All Parents room per season
            var allParentsRoom = await db.ChatRooms
                .FirstOrDefaultAsync(r => r.Type == ChatRoomType.AllParents && r.SeasonId == seasonId);
            if (allParentsRoom == null)
            {
                db.ChatRooms.Add(new ChatRoom
                {
                    Name = "All Parents",
                    Type = ChatRoomType.AllParents,
                    SeasonId = seasonId
                });
            }

            // One room per team level
            foreach (var level in levels)
            {
                var levelRoom = await db.ChatRooms
                    .FirstOrDefaultAsync(r => r.Type == ChatRoomType.Level && r.TeamLevelId == level.Id);
                if (levelRoom == null)
                {
                    db.ChatRooms.Add(new ChatRoom
                    {
                        Name = level.Name,
                        Type = ChatRoomType.Level,
                        SeasonId = seasonId,
                        TeamLevelId = level.Id
                    });
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task<List<ChatRoom>> GetRoomsForUser(string userId, bool isEmployee)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            if (isEmployee)
            {
                return await db.ChatRooms
                    .Include(r => r.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
                    .OrderByDescending(r => r.Messages.Max(m => (DateTime?)m.CreatedAt) ?? r.CreatedAt)
                    .ToListAsync();
            }

            // Parent: get athlete team level IDs
            var athleteTeamLevelIds = await db.ParentLinks
                .Where(pl => pl.ParentUserId == userId && pl.IsClaimed)
                .Join(db.Athletes, pl => pl.AthleteId, a => a.Id, (pl, a) => a.TeamLevelId)
                .Distinct()
                .ToListAsync();

            return await db.ChatRooms
                .Include(r => r.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
                .Where(r => r.Type == ChatRoomType.AllParents
                    || (r.Type == ChatRoomType.Level && r.TeamLevelId.HasValue && athleteTeamLevelIds.Contains(r.TeamLevelId.Value)))
                .OrderByDescending(r => r.Messages.Max(m => (DateTime?)m.CreatedAt) ?? r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetMessages(int roomId, int take = 50, int skip = 0)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.ChatMessages
                .Where(m => m.ChatRoomId == roomId)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Take(take)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task SendMessage(int roomId, string userId, string userName, string content, string? imageData = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.ChatMessages.Add(new ChatMessage
            {
                ChatRoomId = roomId,
                SenderUserId = userId,
                SenderName = userName,
                Content = content,
                ImageData = imageData
            });
            await db.SaveChangesAsync();
        }

        public async Task MarkAsRead(int roomId, string userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var member = await db.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.ChatRoomId == roomId && m.UserId == userId);
            if (member == null)
            {
                db.ChatRoomMembers.Add(new ChatRoomMember
                {
                    ChatRoomId = roomId,
                    UserId = userId,
                    LastReadAt = DateTime.UtcNow
                });
            }
            else
            {
                member.LastReadAt = DateTime.UtcNow;
            }
            await db.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCount(string userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var memberRecords = await db.ChatRoomMembers
                .Where(m => m.UserId == userId)
                .ToDictionaryAsync(m => m.ChatRoomId, m => m.LastReadAt);

            var roomIds = await db.ChatRooms.Select(r => r.Id).ToListAsync();

            int total = 0;
            foreach (var roomId in roomIds)
            {
                DateTime? lastRead = memberRecords.ContainsKey(roomId) ? memberRecords[roomId] : null;

                var query = db.ChatMessages
                    .Where(m => m.ChatRoomId == roomId && m.SenderUserId != userId);

                if (lastRead.HasValue)
                {
                    query = query.Where(m => m.CreatedAt > lastRead.Value);
                }

                total += await query.CountAsync();
            }

            return total;
        }
    }
}
