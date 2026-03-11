using GymBudgetApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymBudgetApp
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Season> Seasons { get; set; }
        public DbSet<Meet> Meets { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<BudgetLineItem> BudgetLineItems { get; set; }
        public DbSet<PerDiemEntry> PerDiemEntries { get; set; }
        public DbSet<TeamLevel> TeamLevels { get; set; }
        public DbSet<AthleteItem> AthleteItems { get; set; }
        public DbSet<MileageEntry> MileageEntries { get; set; }
        public DbSet<UserImportPin> UserImportPins { get; set; }
        public DbSet<CoachMeetAssignment> CoachMeetAssignments { get; set; }
        public DbSet<SeasonGroup> SeasonGroups { get; set; }
        public DbSet<MeetGroupAssignment> MeetGroupAssignments { get; set; }
        public DbSet<CoachGroupAssignment> CoachGroupAssignments { get; set; }
        public DbSet<TeamLevelGroupAssignment> TeamLevelGroupAssignments { get; set; }
        public DbSet<MeetTeamLevelAssignment> MeetTeamLevelAssignments { get; set; }
        public DbSet<SharedFee> SharedFees { get; set; }
        public DbSet<SeasonNote> SeasonNotes { get; set; }
        public DbSet<SharedFeeTeamLevelAssignment> SharedFeeTeamLevelAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BudgetLineItem>()
                .Property(b => b.Rate)
                .HasColumnType("decimal(18,2)");

            builder.Entity<BudgetLineItem>()
                .Property(b => b.Quantity)
                .HasColumnType("decimal(18,2)");

            builder.Entity<PerDiemEntry>()
                .Property(p => p.Rate)
                .HasColumnType("decimal(18,2)");

            builder.Entity<AthleteItem>()
                .Property(a => a.Cost)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Meet>()
                .Property(m => m.BudgetAmount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<SharedFee>()
                .Property(sf => sf.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<MileageEntry>()
                .Property(m => m.Miles)
                .HasColumnType("decimal(18,2)");

            builder.Entity<MileageEntry>()
                .Property(m => m.RatePerMile)
                .HasColumnType("decimal(18,2)");

            builder.Entity<CoachMeetAssignment>()
                .HasIndex(cma => new { cma.CoachId, cma.MeetId })
                .IsUnique();

            builder.Entity<Coach>()
                .HasOne(c => c.SeasonGroup)
                .WithMany(g => g.Coaches)
                .HasForeignKey(c => c.SeasonGroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Meet>()
                .HasOne(m => m.SeasonGroup)
                .WithMany(g => g.Meets)
                .HasForeignKey(m => m.SeasonGroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BudgetLineItem>()
                .HasOne(b => b.SeasonGroup)
                .WithMany(g => g.BudgetLineItems)
                .HasForeignKey(b => b.SeasonGroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<MeetGroupAssignment>()
                .HasIndex(mga => new { mga.MeetId, mga.SeasonGroupId })
                .IsUnique();

            builder.Entity<CoachGroupAssignment>()
                .HasIndex(cga => new { cga.CoachId, cga.SeasonGroupId })
                .IsUnique();

            builder.Entity<TeamLevelGroupAssignment>()
                .HasIndex(tga => new { tga.TeamLevelId, tga.SeasonGroupId })
                .IsUnique();

            builder.Entity<MeetTeamLevelAssignment>()
                .HasIndex(mta => new { mta.MeetId, mta.TeamLevelId })
                .IsUnique();

            builder.Entity<SharedFeeTeamLevelAssignment>()
                .HasIndex(sfta => new { sfta.SharedFeeId, sfta.TeamLevelId })
                .IsUnique();
        }
    }
}
