using GymBudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymBudgetApp.Services
{
    public class AuditService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public AuditService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task LogAsync(string userId, string userEmail, string action, string details)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                UserEmail = userEmail,
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
    }
}
