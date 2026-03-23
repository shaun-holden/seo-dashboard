using GymBudgetApp;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260323221500_EnforcePaymentSeasonForeignKey")]
    public partial class EnforcePaymentSeasonForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Payments
                SET SeasonId = (
                    SELECT tl.SeasonId
                    FROM Athletes a
                    JOIN TeamLevels tl ON tl.Id = a.TeamLevelId
                    WHERE a.Id = Payments.AthleteId
                )
                WHERE SeasonId IS NULL
                   OR NOT EXISTS (
                       SELECT 1
                       FROM Seasons s
                       WHERE s.Id = Payments.SeasonId
                   );
                """);

            migrationBuilder.Sql("""
                UPDATE Payments
                SET SeasonId = (
                    SELECT Id
                    FROM Seasons
                    ORDER BY IsActive DESC, IsPublished DESC, CreatedAt DESC, Id DESC
                    LIMIT 1
                )
                WHERE SeasonId IS NULL
                   OR NOT EXISTS (
                       SELECT 1
                       FROM Seasons s
                       WHERE s.Id = Payments.SeasonId
                   );
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Seasons_SeasonId",
                table: "Payments",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Seasons_SeasonId",
                table: "Payments");
        }
    }
}
