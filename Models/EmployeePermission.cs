using System.ComponentModel.DataAnnotations;

namespace GymBudgetApp.Models
{
    public class EmployeePermission
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public bool CanViewBudget { get; set; } = true;
        public bool CanEditBudget { get; set; }
        public bool CanViewAthletes { get; set; } = true;
        public bool CanEditAthletes { get; set; }
        public bool CanViewCoaches { get; set; } = true;
        public bool CanViewMeets { get; set; } = true;
        public bool CanViewReports { get; set; } = true;
        public bool CanViewPayments { get; set; }
        public bool CanManageRoster { get; set; }
        public bool CanViewNotes { get; set; } = true;
    }
}
