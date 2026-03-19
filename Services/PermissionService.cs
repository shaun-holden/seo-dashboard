using GymBudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymBudgetApp.Services
{
    public class PermissionService
    {
        private readonly AppDbContext _db;
        private bool _loaded;
        private bool _isAdmin;

        public bool CanViewBudget { get; private set; } = true;
        public bool CanEditBudget { get; private set; } = true;
        public bool CanViewAthletes { get; private set; } = true;
        public bool CanEditAthletes { get; private set; } = true;
        public bool CanViewCoaches { get; private set; } = true;
        public bool CanViewMeets { get; private set; } = true;
        public bool CanViewReports { get; private set; } = true;
        public bool CanViewPayments { get; private set; } = true;
        public bool CanManageRoster { get; private set; } = true;
        public bool CanViewNotes { get; private set; } = true;

        public PermissionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task LoadAsync(string? userId, string? email, bool isAdmin)
        {
            if (_loaded) return;
            _loaded = true;
            _isAdmin = isAdmin;

            // Admin always has full access
            if (_isAdmin)
                return;

            if (string.IsNullOrEmpty(userId))
                return;

            var perm = await _db.Set<EmployeePermission>()
                .FirstOrDefaultAsync(p => p.UserId == userId);

            // If no permission record exists, default to all true (full access)
            if (perm == null)
                return;

            CanViewBudget = perm.CanViewBudget;
            CanEditBudget = perm.CanEditBudget;
            CanViewAthletes = perm.CanViewAthletes;
            CanEditAthletes = perm.CanEditAthletes;
            CanViewCoaches = perm.CanViewCoaches;
            CanViewMeets = perm.CanViewMeets;
            CanViewReports = perm.CanViewReports;
            CanViewPayments = perm.CanViewPayments;
            CanManageRoster = perm.CanManageRoster;
            CanViewNotes = perm.CanViewNotes;
        }
    }
}
