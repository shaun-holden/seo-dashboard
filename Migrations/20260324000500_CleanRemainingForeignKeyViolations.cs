using GymBudgetApp;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260324000500_CleanRemainingForeignKeyViolations")]
    public partial class CleanRemainingForeignKeyViolations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM SeasonGroups
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Seasons s
                    WHERE s.Id = SeasonGroups.SeasonId
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM ChatMessages
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM ChatRooms cr
                    WHERE cr.Id = ChatMessages.ChatRoomId
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM ChatRoomMembers
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM ChatRooms cr
                    WHERE cr.Id = ChatRoomMembers.ChatRoomId
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM GymnastApparelSelections
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Gymnasts g
                    WHERE g.Id = GymnastApparelSelections.GymnastId
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM GymnastCompetitionSelections
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Gymnasts g
                    WHERE g.Id = GymnastCompetitionSelections.GymnastId
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM ParentLinks
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Gymnasts g
                    WHERE g.Id = ParentLinks.AthleteId
                );
                """);

            migrationBuilder.Sql("""
                DELETE FROM CommitmentSignatures
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Gymnasts g
                    WHERE g.Id = CommitmentSignatures.AthleteId
                );
                """);

            migrationBuilder.Sql("""
                CREATE TABLE "ParentLinks_temp" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_ParentLinks" PRIMARY KEY AUTOINCREMENT,
                    "InviteCode" TEXT NOT NULL,
                    "AthleteId" INTEGER NOT NULL,
                    "ParentUserId" TEXT NULL,
                    "IsClaimed" INTEGER NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "ClaimedAt" TEXT NULL,
                    "AutoPayEnabled" INTEGER NOT NULL,
                    "StripeCustomerId" TEXT NULL,
                    "StripeSubscriptionId" TEXT NULL,
                    "UseExternalBilling" INTEGER NOT NULL,
                    CONSTRAINT "FK_ParentLinks_Gymnasts_AthleteId" FOREIGN KEY ("AthleteId") REFERENCES "Gymnasts" ("Id") ON DELETE CASCADE
                );
                """);

            migrationBuilder.Sql("""
                INSERT INTO "ParentLinks_temp" (
                    "Id",
                    "InviteCode",
                    "AthleteId",
                    "ParentUserId",
                    "IsClaimed",
                    "CreatedAt",
                    "ClaimedAt",
                    "AutoPayEnabled",
                    "StripeCustomerId",
                    "StripeSubscriptionId",
                    "UseExternalBilling"
                )
                SELECT
                    "Id",
                    "InviteCode",
                    "AthleteId",
                    "ParentUserId",
                    "IsClaimed",
                    "CreatedAt",
                    "ClaimedAt",
                    "AutoPayEnabled",
                    "StripeCustomerId",
                    "StripeSubscriptionId",
                    "UseExternalBilling"
                FROM "ParentLinks";
                """);

            migrationBuilder.Sql("""DROP TABLE "ParentLinks";""");
            migrationBuilder.Sql("""ALTER TABLE "ParentLinks_temp" RENAME TO "ParentLinks";""");

            migrationBuilder.CreateIndex(
                name: "IX_ParentLinks_AthleteId",
                table: "ParentLinks",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentLinks_InviteCode",
                table: "ParentLinks",
                column: "InviteCode",
                unique: true);

            migrationBuilder.Sql("""
                CREATE TABLE "CommitmentSignatures_temp" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_CommitmentSignatures" PRIMARY KEY AUTOINCREMENT,
                    "CommitmentFormId" INTEGER NOT NULL,
                    "AthleteId" INTEGER NOT NULL,
                    "ParentUserId" TEXT NOT NULL,
                    "ParentEmail" TEXT NOT NULL,
                    "SignatureName" TEXT NOT NULL,
                    "SignedDate" TEXT NOT NULL,
                    "Initials" TEXT NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    CONSTRAINT "FK_CommitmentSignatures_CommitmentForms_CommitmentFormId" FOREIGN KEY ("CommitmentFormId") REFERENCES "CommitmentForms" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_CommitmentSignatures_Gymnasts_AthleteId" FOREIGN KEY ("AthleteId") REFERENCES "Gymnasts" ("Id") ON DELETE CASCADE
                );
                """);

            migrationBuilder.Sql("""
                INSERT INTO "CommitmentSignatures_temp" (
                    "Id",
                    "CommitmentFormId",
                    "AthleteId",
                    "ParentUserId",
                    "ParentEmail",
                    "SignatureName",
                    "SignedDate",
                    "Initials",
                    "CreatedAt"
                )
                SELECT
                    "Id",
                    "CommitmentFormId",
                    "AthleteId",
                    "ParentUserId",
                    "ParentEmail",
                    "SignatureName",
                    "SignedDate",
                    "Initials",
                    "CreatedAt"
                FROM "CommitmentSignatures";
                """);

            migrationBuilder.Sql("""DROP TABLE "CommitmentSignatures";""");
            migrationBuilder.Sql("""ALTER TABLE "CommitmentSignatures_temp" RENAME TO "CommitmentSignatures";""");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentSignatures_AthleteId",
                table: "CommitmentSignatures",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentSignatures_CommitmentFormId",
                table: "CommitmentSignatures",
                column: "CommitmentFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE "ParentLinks_temp" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_ParentLinks" PRIMARY KEY AUTOINCREMENT,
                    "InviteCode" TEXT NOT NULL,
                    "AthleteId" INTEGER NOT NULL,
                    "ParentUserId" TEXT NULL,
                    "IsClaimed" INTEGER NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "ClaimedAt" TEXT NULL,
                    "AutoPayEnabled" INTEGER NOT NULL,
                    "StripeCustomerId" TEXT NULL,
                    "StripeSubscriptionId" TEXT NULL,
                    "UseExternalBilling" INTEGER NOT NULL,
                    CONSTRAINT "FK_ParentLinks_Athletes_AthleteId" FOREIGN KEY ("AthleteId") REFERENCES "Athletes" ("Id") ON DELETE CASCADE
                );
                """);

            migrationBuilder.Sql("""
                INSERT INTO "ParentLinks_temp" (
                    "Id",
                    "InviteCode",
                    "AthleteId",
                    "ParentUserId",
                    "IsClaimed",
                    "CreatedAt",
                    "ClaimedAt",
                    "AutoPayEnabled",
                    "StripeCustomerId",
                    "StripeSubscriptionId",
                    "UseExternalBilling"
                )
                SELECT
                    "Id",
                    "InviteCode",
                    "AthleteId",
                    "ParentUserId",
                    "IsClaimed",
                    "CreatedAt",
                    "ClaimedAt",
                    "AutoPayEnabled",
                    "StripeCustomerId",
                    "StripeSubscriptionId",
                    "UseExternalBilling"
                FROM "ParentLinks";
                """);

            migrationBuilder.DropIndex(
                name: "IX_ParentLinks_InviteCode",
                table: "ParentLinks");

            migrationBuilder.DropIndex(
                name: "IX_ParentLinks_AthleteId",
                table: "ParentLinks");

            migrationBuilder.Sql("""DROP TABLE "ParentLinks";""");
            migrationBuilder.Sql("""ALTER TABLE "ParentLinks_temp" RENAME TO "ParentLinks";""");

            migrationBuilder.CreateIndex(
                name: "IX_ParentLinks_AthleteId",
                table: "ParentLinks",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentLinks_InviteCode",
                table: "ParentLinks",
                column: "InviteCode",
                unique: true);

            migrationBuilder.Sql("""
                CREATE TABLE "CommitmentSignatures_temp" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_CommitmentSignatures" PRIMARY KEY AUTOINCREMENT,
                    "CommitmentFormId" INTEGER NOT NULL,
                    "AthleteId" INTEGER NOT NULL,
                    "ParentUserId" TEXT NOT NULL,
                    "ParentEmail" TEXT NOT NULL,
                    "SignatureName" TEXT NOT NULL,
                    "SignedDate" TEXT NOT NULL,
                    "Initials" TEXT NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    CONSTRAINT "FK_CommitmentSignatures_Athletes_AthleteId" FOREIGN KEY ("AthleteId") REFERENCES "Athletes" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_CommitmentSignatures_CommitmentForms_CommitmentFormId" FOREIGN KEY ("CommitmentFormId") REFERENCES "CommitmentForms" ("Id") ON DELETE CASCADE
                );
                """);

            migrationBuilder.Sql("""
                INSERT INTO "CommitmentSignatures_temp" (
                    "Id",
                    "CommitmentFormId",
                    "AthleteId",
                    "ParentUserId",
                    "ParentEmail",
                    "SignatureName",
                    "SignedDate",
                    "Initials",
                    "CreatedAt"
                )
                SELECT
                    "Id",
                    "CommitmentFormId",
                    "AthleteId",
                    "ParentUserId",
                    "ParentEmail",
                    "SignatureName",
                    "SignedDate",
                    "Initials",
                    "CreatedAt"
                FROM "CommitmentSignatures";
                """);

            migrationBuilder.DropIndex(
                name: "IX_CommitmentSignatures_CommitmentFormId",
                table: "CommitmentSignatures");

            migrationBuilder.DropIndex(
                name: "IX_CommitmentSignatures_AthleteId",
                table: "CommitmentSignatures");

            migrationBuilder.Sql("""DROP TABLE "CommitmentSignatures";""");
            migrationBuilder.Sql("""ALTER TABLE "CommitmentSignatures_temp" RENAME TO "CommitmentSignatures";""");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentSignatures_AthleteId",
                table: "CommitmentSignatures",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentSignatures_CommitmentFormId",
                table: "CommitmentSignatures",
                column: "CommitmentFormId");
        }
    }
}
