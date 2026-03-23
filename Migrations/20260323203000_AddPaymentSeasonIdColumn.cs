using GymBudgetApp;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260323203000_AddPaymentSeasonIdColumn")]
    public partial class AddPaymentSeasonIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Payments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE Payments
                SET SeasonId = (
                    SELECT tl.SeasonId
                    FROM Athletes a
                    JOIN TeamLevels tl ON tl.Id = a.TeamLevelId
                    WHERE a.Id = Payments.AthleteId
                )
                WHERE SeasonId IS NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE Payments
                SET SeasonId = (
                    SELECT Id
                    FROM Seasons
                    ORDER BY IsActive DESC, IsPublished DESC, CreatedAt DESC, Id DESC
                    LIMIT 1
                )
                WHERE SeasonId IS NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SeasonId_AthleteId",
                table: "Payments",
                columns: new[] { "SeasonId", "AthleteId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_SeasonId_AthleteId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Payments");
        }
    }
}
