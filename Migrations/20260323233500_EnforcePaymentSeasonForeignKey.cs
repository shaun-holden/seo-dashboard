using GymBudgetApp;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260323233500_EnforcePaymentSeasonForeignKey")]
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

            migrationBuilder.Sql("""
                CREATE TABLE "Payments_temp" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_Payments" PRIMARY KEY AUTOINCREMENT,
                    "SeasonId" INTEGER NULL,
                    "AthleteId" INTEGER NOT NULL,
                    "PayerUserId" TEXT NOT NULL,
                    "Amount" decimal(18,2) NOT NULL,
                    "Status" INTEGER NOT NULL,
                    "Type" INTEGER NOT NULL DEFAULT 0,
                    "StripeSessionId" TEXT NULL,
                    "StripePaymentIntentId" TEXT NULL,
                    "Description" TEXT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "PaidAt" TEXT NULL,
                    CONSTRAINT "FK_Payments_Athletes_AthleteId" FOREIGN KEY ("AthleteId") REFERENCES "Athletes" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_Payments_Seasons_SeasonId" FOREIGN KEY ("SeasonId") REFERENCES "Seasons" ("Id") ON DELETE SET NULL
                );
                """);

            migrationBuilder.Sql("""
                INSERT INTO "Payments_temp" (
                    "Id",
                    "SeasonId",
                    "AthleteId",
                    "PayerUserId",
                    "Amount",
                    "Status",
                    "Type",
                    "StripeSessionId",
                    "StripePaymentIntentId",
                    "Description",
                    "CreatedAt",
                    "PaidAt"
                )
                SELECT
                    "Id",
                    "SeasonId",
                    "AthleteId",
                    "PayerUserId",
                    "Amount",
                    "Status",
                    "Type",
                    "StripeSessionId",
                    "StripePaymentIntentId",
                    "Description",
                    "CreatedAt",
                    "PaidAt"
                FROM "Payments";
                """);

            migrationBuilder.Sql("""DROP TABLE "Payments";""");
            migrationBuilder.Sql("""ALTER TABLE "Payments_temp" RENAME TO "Payments";""");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AthleteId",
                table: "Payments",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SeasonId_AthleteId",
                table: "Payments",
                columns: new[] { "SeasonId", "AthleteId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE "Payments_temp" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_Payments" PRIMARY KEY AUTOINCREMENT,
                    "SeasonId" INTEGER NULL,
                    "AthleteId" INTEGER NOT NULL,
                    "PayerUserId" TEXT NOT NULL,
                    "Amount" decimal(18,2) NOT NULL,
                    "Status" INTEGER NOT NULL,
                    "Type" INTEGER NOT NULL DEFAULT 0,
                    "StripeSessionId" TEXT NULL,
                    "StripePaymentIntentId" TEXT NULL,
                    "Description" TEXT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "PaidAt" TEXT NULL,
                    CONSTRAINT "FK_Payments_Athletes_AthleteId" FOREIGN KEY ("AthleteId") REFERENCES "Athletes" ("Id") ON DELETE CASCADE
                );
                """);

            migrationBuilder.Sql("""
                INSERT INTO "Payments_temp" (
                    "Id",
                    "SeasonId",
                    "AthleteId",
                    "PayerUserId",
                    "Amount",
                    "Status",
                    "Type",
                    "StripeSessionId",
                    "StripePaymentIntentId",
                    "Description",
                    "CreatedAt",
                    "PaidAt"
                )
                SELECT
                    "Id",
                    "SeasonId",
                    "AthleteId",
                    "PayerUserId",
                    "Amount",
                    "Status",
                    "Type",
                    "StripeSessionId",
                    "StripePaymentIntentId",
                    "Description",
                    "CreatedAt",
                    "PaidAt"
                FROM "Payments";
                """);

            migrationBuilder.DropIndex(
                name: "IX_Payments_SeasonId_AthleteId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AthleteId",
                table: "Payments");

            migrationBuilder.Sql("""DROP TABLE "Payments";""");
            migrationBuilder.Sql("""ALTER TABLE "Payments_temp" RENAME TO "Payments";""");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AthleteId",
                table: "Payments",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SeasonId_AthleteId",
                table: "Payments",
                columns: new[] { "SeasonId", "AthleteId" });
        }
    }
}
