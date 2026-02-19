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

            builder.Entity<MileageEntry>()
                .Property(m => m.Miles)
                .HasColumnType("decimal(18,2)");

            builder.Entity<MileageEntry>()
                .Property(m => m.RatePerMile)
                .HasColumnType("decimal(18,2)");
        }
    }
}
