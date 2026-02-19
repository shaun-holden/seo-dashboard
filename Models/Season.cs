namespace GymBudgetApp.Models
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AthleteCount { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ICollection<Meet> Meets { get; set; } = new List<Meet>();
        public ICollection<Coach> Coaches { get; set; } = new List<Coach>();
        public ICollection<TeamLevel> TeamLevels { get; set; } = new List<TeamLevel>();
    }
}